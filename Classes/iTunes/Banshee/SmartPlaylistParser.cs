// 
// SmartPlaylistParser.cs: Parses smart playlist binary data from iTunes
// 
// Author:
//   Scott Peterson (lunchtimemama at gmail)
// 

using System;
using System.Collections.Generic;
using System.Text;

namespace Banshee.Plugins.iTunesImporter
{
	internal struct SmartPlaylist
	{
		public string Query, Ignore, OrderBy, Name, Output;
		public uint LimitNumber;
		public byte LimitMethod;
	}

	internal static partial class SmartPlaylistParser
	{
        private delegate bool KindEvalDel(Kind kind, string query);

        // INFO OFFSETS
        //
        // Offsets for bytes which...
        const int MATCHBOOLOFFSET = 1;           // determin whether logical matching is to be performed - Absolute offset
        const int LIMITBOOLOFFSET = 2;           // determin whether results are limited - Absolute offset
        const int LIMITMETHODOFFSET = 3;         // determin by what criteria the results are limited - Absolute offset
        const int SELECTIONMETHODOFFSET = 7;     // determin by what criteria limited playlists are populated - Absolute offset
        const int LIMITINTOFFSET = 11;           // determin the limited - Absolute offset
        const int SELECTIONMETHODSIGNOFFSET = 13;// determin whether certain selection methods are "most" or "least" - Absolute offset 

        // CRITERIA OFFSETS
        //
        // Offsets for bytes which...
        const int LOGICTYPEOFFSET = 15;   // determin whether all or any criteria must match - Absolute offset
        const int FIELDOFFSET = 139;      // determin what is being matched (Artist, Album, &c) - Absolute offset
        const int LOGICSIGNOFFSET = 1;    // determin whether the matching rule is positive or negative (e.g., is vs. is not) - Relative offset from FIELDOFFSET
        const int LOGICRULEOFFSET = 4;    // determin the kind of logic used (is, contains, begins, &c) - Relative offset from FIELDOFFSET
        const int STRINGOFFSET = 54;      // begin string data - Relative offset from FIELDOFFSET
        const int INTAOFFSET = 60;        // begin the first int - Relative offset from FIELDOFFSET
        const int INTBOFFSET = 24;        // begin the second int - Relative offset from INTAOFFSET
        const int TIMEMULTIPLEOFFSET = 76;// begin the int with the multiple of time - Relative offset from FIELDOFFSET
        const int TIMEVALUEOFFSET = 68;   // begin the inverse int with the value of time - Relative offset from FIELDOFFSET

        const int INTLENGTH = 64;       // The length on a int criteria starting at the first int
        static DateTime STARTOFTIME = new DateTime(1904, 1, 1); // Dates are recorded as seconds since Jan 1, 1904

        static bool or, again;
        static string conjunctionOutput, conjunctionQuery, output, query, ignore;
        static int offset, logicSignOffset,logicRulesOffset, stringOffset, intAOffset, intBOffset,
            timeMultipleOffset, timeValueOffset;
        static byte[] info, criteria;

        static KindEvalDel KindEval;

        public static SmartPlaylist Parse(byte[] i, byte[] c)
        {
            info = i;
			criteria = c;
			SmartPlaylist result = new SmartPlaylist();
			offset = FIELDOFFSET;
			output = "";
			query = "";
			ignore = "";

			if(info[MATCHBOOLOFFSET] == 1) {
				or = (criteria[LOGICTYPEOFFSET] == 1) ? true : false;
				if(or) {
					conjunctionQuery = " OR ";
					conjunctionOutput = " or\n";
				} else {
					conjunctionQuery = " AND ";
					conjunctionOutput = " and\n";
				}
				do {
					again = false;
					logicSignOffset = offset + LOGICSIGNOFFSET;
					logicRulesOffset = offset + LOGICRULEOFFSET;
					stringOffset = offset + STRINGOFFSET;
					intAOffset = offset + INTAOFFSET;
					intBOffset = intAOffset + INTBOFFSET;
					timeMultipleOffset = offset + TIMEMULTIPLEOFFSET;
					timeValueOffset = offset + TIMEVALUEOFFSET;
					
					if(Enum.IsDefined(typeof(StringFields), (int)criteria[offset])) {
						ProcessStringField();
					} else if(Enum.IsDefined(typeof(IntFields), (int)criteria[offset])) {
						ProcessIntField();
					} else if(Enum.IsDefined(typeof(DateFields), (int)criteria[offset])) {
						ProcessDateField();
					} else {
						ignore += "Not processed";
				    }
				}
				while(again);
			}
			result.Output = output;
			result.Query = query;
			result.Ignore = ignore;
			if(info[LIMITBOOLOFFSET] == 1) {
				uint limit = BytesToUInt(info, LIMITINTOFFSET);
				result.LimitNumber = (info[LIMITMETHODOFFSET] == (byte)LimitMethods.GB) ? limit * 1024 : limit;
				if(output.Length > 0) {
				  	output += "\n";
				}
				output += "Limited to " + limit.ToString() + " " +
					Enum.GetName(typeof(LimitMethods), (int)info[LIMITMETHODOFFSET]) + " selected by ";
				switch(info[LIMITMETHODOFFSET]) {
				case (byte)LimitMethods.Items:
					result.LimitMethod = 0;
					break;
				case (byte)LimitMethods.Minutes:
					result.LimitMethod = 1;
					break;
				case (byte)LimitMethods.Hours:
					result.LimitMethod = 2;
					break;
				case (byte)LimitMethods.MB:
					result.LimitMethod = 3;
					break;
				case (byte)LimitMethods.GB:
					goto case (byte)LimitMethods.MB;
				}
				switch(info[SELECTIONMETHODOFFSET]) {
				case (byte)SelectionMethods.Random:
					output += "random";
					result.OrderBy = "RANDOM()";
					break;
				case (byte)SelectionMethods.HighestRating:
					output += "highest rated";
					result.OrderBy = "Rating DESC";
					break;
				case (byte)SelectionMethods.LowestRating:
					output += "lowest rated";
					result.OrderBy = "Rating ASC";
					break;
				case (byte)SelectionMethods.RecentlyPlayed:
					output += (info[SELECTIONMETHODSIGNOFFSET] == 0)
						? "most recently played" : "least recently played";
					result.OrderBy = (info[SELECTIONMETHODSIGNOFFSET] == 0)
						? "LastPlayedStamp DESC" : "LastPlayedStamp ASC";
					break;
				case (byte)SelectionMethods.OftenPlayed:
					output += (info[SELECTIONMETHODSIGNOFFSET] == 0)
						? "most often played" : "least often played";
					result.OrderBy = (info[SELECTIONMETHODSIGNOFFSET] == 0)
						? "NumberOfPlays DESC" : "NumberOfPlays ASC";
					break;
				case (byte)SelectionMethods.RecentlyAdded:
					output += (info[SELECTIONMETHODSIGNOFFSET] == 0)
						? "most recently added" : "least recently added";
					result.OrderBy = (info[SELECTIONMETHODSIGNOFFSET] == 0)
						? "DateAddedStamp DESC" : "DateAddedStamp ASC";
					break;
				default:
					result.OrderBy = Enum.GetName(typeof(SelectionMethods), (int)info[SELECTIONMETHODOFFSET]);
					break;
				}
			}
			if(ignore.Length > 0) {
				output += "\n\nIGNORING:\n" + ignore;
			}
			
			if(query.Length > 0) {
				output += "\n\nQUERY:\n" + query;
			}
			return result;
        }

        private static void ProcessStringField()
        {
            bool end = false;
       		string workingOutput = Enum.GetName(typeof(StringFields), criteria[offset]);
       		string workingQuery = "(lower(" + Enum.GetName(typeof(StringFields), criteria[offset]) + ")";
       		switch(criteria[logicRulesOffset]) {
       		case (byte)LogicRule.Contains:
       			if((criteria[logicSignOffset] == (byte)LogicSign.StringPositive)) {
       			  	workingOutput += " contains ";
       			  	workingQuery += " LIKE '%";
       		  	} else {
       				workingOutput += " does not contain ";
       				workingQuery += " NOT LIKE '%";
       			}
       			if(criteria[offset] == (byte)StringFields.Kind) {
       				KindEval = delegate(Kind kind, string query) {
       					return (kind.Name.IndexOf(query) != -1);
       				};
       			}
       			end = true;
       			break;
       		case (byte)LogicRule.Is:
       			if((criteria[logicSignOffset] == (byte)LogicSign.StringPositive)) {
       				workingOutput += " is ";
       				workingQuery += " = '";
       			} else {
       				workingOutput += " is not ";
       				workingQuery += " != '";
       			}
       			if(criteria[offset] == (byte)StringFields.Kind) {
       				KindEval = delegate(Kind kind, string query) {
       					return (kind.Name == query);
       				};
       			}
       			break;
       		case (byte)LogicRule.Starts:
       			workingOutput += " starts with ";
       			workingQuery += " LIKE '";
       			if(criteria[offset] == (byte)StringFields.Kind) {
       				KindEval = delegate (Kind kind, string query) {
       					return (kind.Name.IndexOf(query) == 0);
       				};
       			}
       			end = true;
       			break;
       		case (byte)LogicRule.Ends:
       			workingOutput += " ends with ";
       			workingQuery += " LIKE '%";
       			if(criteria[offset] == (byte)StringFields.Kind) {
       				KindEval = delegate (Kind kind, string query) {
       					return (kind.Name.IndexOf(query) == (kind.Name.Length - query.Length));
       			    };
       			}
       			break;
            }
            workingOutput += "\"";
            byte[] character = new byte[1];
            string content = "";
            bool onByte = true;
            for(int i = (stringOffset); i < criteria.Length; i++) {
                // Off bytes are 0
                if(onByte) {
                    // If the byte is 0 and it's not the last byte,
                    // we have another condition
                    if(criteria[i] == 0 && i != (criteria.Length - 1)) {
                        again = true;
                        FinishStringField(content, workingOutput, workingQuery, end);
                        offset = i + 2;
                        return;
                    }
                    character[0] = criteria[i];
                    content += Encoding.UTF8.GetString(character);
                }
                onByte = !onByte;
            }
            FinishStringField(content, workingOutput, workingQuery, end);
        }

        private static void FinishStringField(string content, string workingOutput, string workingQuery, bool end)
        {
           	workingOutput += content;
           	workingOutput += "\" ";
           	bool failed = false;
           	if(criteria[offset] == (byte)StringFields.Kind) {
           		workingQuery = "";
        		foreach(Kind kind in Kinds) {
        			if(KindEval(kind, content)) {
        				if(workingQuery.Length > 0) {
	        			    if((query.Length == 0 && !again) || or) {
	        			        workingQuery += " OR ";
	        			    } else {
        	        		    failed = true;
        	        		    break;
        	        		}
	        			}
        				workingQuery += "(lower(Uri)";
        				workingQuery += ((criteria[logicSignOffset] == (byte)LogicSign.StringPositive))
        					? " LIKE '%" + kind.Extension + "')" : " NOT LIKE '%" + kind.Extension + "%')";
        			}
        		}
           	} else {
           		workingQuery += content.ToLower();
           		workingQuery += (end) ? "%')" : "')";
           	}
			if(Enum.IsDefined(typeof(IgnoreStringFields),
				(int)criteria[offset]) || failed) {
			    if(ignore.Length > 0) {
				    ignore += conjunctionOutput;
				}
				ignore += workingOutput;
			} else {
				if(output.Length > 0) {
					output += conjunctionOutput;
				}
				if(query.Length > 0) {
					query += conjunctionQuery;
			    }
				output += workingOutput;
				query += workingQuery;
			}
        }

        private static void ProcessIntField()
        {
       		string workingOutput = Enum.GetName(typeof(IntFields), criteria[offset]);
       		string workingQuery = "(" + Enum.GetName(typeof(IntFields), criteria[offset]);
       		
       		switch(criteria[logicRulesOffset]) {
       		case (byte)LogicRule.Is:
       			if(criteria[logicSignOffset] == (byte)LogicSign.IntPositive) {
       				workingOutput += " is ";
       				workingQuery += " = ";
       			} else {
       				workingOutput += " is not ";
       				workingQuery += " != ";
       			}
       			goto case 255;
       		case (byte)LogicRule.Greater:
       			workingOutput += " is greater than ";
       			workingQuery += " > ";
       			goto case 255;
       		case (byte)LogicRule.Less:
       			workingOutput += " is less than ";
       			workingQuery += " > ";
       			goto case 255;
       		case 255:
       			uint number = (criteria[offset] == (byte)IntFields.Rating)
       				? (BytesToUInt(criteria, intAOffset) / 20) : BytesToUInt(criteria, intAOffset);
       			workingOutput += number.ToString();
       			workingQuery += number.ToString();
       			break;
       		case (byte)LogicRule.Other:
       			if(criteria[logicSignOffset + 2] == 1) {
       				workingOutput += " is in the range of ";
       				workingQuery += " BETWEEN ";
       				uint num = (criteria[offset] == (byte)IntFields.Rating)
       					? (BytesToUInt(criteria, intAOffset) / 20) : BytesToUInt(criteria, intAOffset);
       				workingOutput += num.ToString();
       				workingQuery += num.ToString();
       				workingOutput += " to ";
       				workingQuery += " AND ";
       				num = (criteria[offset] == (byte)IntFields.Rating)
       					? ((BytesToUInt(criteria, intBOffset) - 19) / 20) : BytesToUInt(criteria, intBOffset);
       				workingOutput += num.ToString();
       				workingQuery += num.ToString();
       			}
       			break;
            }
       		workingQuery += ")";
       		if(Enum.IsDefined(typeof(IgnoreIntFields),
       			(int)criteria[offset])) {
       			if(ignore.Length > 0) {
       				ignore += conjunctionOutput;
       			}
       			ignore += workingOutput;
       		} else {
       			if(output.Length > 0) {
       				output += conjunctionOutput;
       			}
       			if(query.Length > 0) {
       				query += conjunctionQuery;
       			}
       			output += workingOutput;
       			query += workingQuery;
       		}
       		offset = intAOffset + INTLENGTH;
       		if(criteria.Length > offset) {
       			again = true;
            }
        }

        private static void ProcessDateField()
        {
            bool isIgnore = false;
           	string workingOutput = Enum.GetName(typeof(DateFields), criteria[offset]);
           	string workingQuery = "((strftime(\"%s\", current_timestamp) - DateAddedStamp + 3600)";
           	switch(criteria[logicRulesOffset]) {
           	case (byte)LogicRule.Greater:
           		workingOutput += " is after ";
           		workingQuery += " > ";
           		goto case 255;
           	case (byte)LogicRule.Less:
           		workingOutput += " is before ";
           		workingQuery += " > ";
           		goto case 255;
           	case 255:
           		isIgnore = true;
           		DateTime time = BytesToDateTime(criteria, intAOffset);
           		workingOutput += time.ToString();
           		workingQuery += ((int)DateTime.Now.Subtract(time).TotalSeconds).ToString();
           		break;
           	case (byte)LogicRule.Other:
           		if(criteria[logicSignOffset + 2] == 1) {
           			isIgnore = true;
           			DateTime t2 = BytesToDateTime(criteria, intAOffset);
           			DateTime t1 = BytesToDateTime(criteria, intBOffset);
           			if(criteria[logicSignOffset] == (byte)LogicSign.IntPositive) {
           				workingOutput += " is in the range of ";
           				workingQuery += " BETWEEN " +
           					((int)DateTime.Now.Subtract(t1).TotalSeconds).ToString() +
           					" AND " +
           					((int)DateTime.Now.Subtract(t2).TotalSeconds).ToString();
           			} else {
           				workingOutput += " is not in the range of ";
           			}
           			workingOutput += t1.ToString();
           			workingOutput += " to ";
           			workingOutput += t2.ToString();
           		} else if(criteria[logicSignOffset + 2] == 2) {
           			if(criteria[logicSignOffset] == (byte)LogicSign.IntPositive) {
           				workingOutput += " is in the last ";
           				workingQuery += " < ";
           			} else {
           				workingOutput += " is not in the last ";
           				workingQuery += " > ";
           			}
           			uint t = InverseBytesToUInt(criteria, timeValueOffset);
           			uint multiple = BytesToUInt(criteria, timeMultipleOffset);
           			workingQuery += (t * multiple).ToString();
           			workingOutput += t.ToString() + " ";
           			switch(multiple) {
           			case 86400:
           				workingOutput += "days";
           				break;
           			case 604800:
           				workingOutput += "weeks";
           				break;
           			case 2628000:
           				workingOutput += "months";
           			 	break;
           			}
           		}
           		break;
            }
			workingQuery += ")";
			if(isIgnore || Enum.IsDefined(typeof(IgnoreDateFields), (int)criteria[offset])) {
				if(ignore.Length > 0) {
				    ignore += conjunctionOutput;
				}
				ignore += workingOutput;
			} else {
				if(output.Length > 0) {
					output += conjunctionOutput;
				}
				output += workingOutput;
				if(query.Length > 0) {
					query += conjunctionQuery;
				}
				query += workingQuery;
			}          
			offset = intAOffset + INTLENGTH;
			if(criteria.Length > offset) {
				again = true;
			}
        }

        /// <summary>
        /// Converts 4 bytes to a uint
        /// </summary>
        /// <param name="byteArray">A byte array</param>
        /// <param name="offset">Should be the byte of the uint with the 0th-power position</param>
        /// <returns></returns>
        private static uint BytesToUInt(byte[] byteArray, int offset)
        {
			uint output = 0;
			for (byte i = 0; i <= 4; i++) {
				output += (uint)(byteArray[offset - i] * Math.Pow(2, (8 * i)));
		    }
			return output;
        }

        private static uint InverseBytesToUInt(byte[] byteArray, int offset)
        {
			uint output = 0;
			for (byte i = 0; i <= 4; i++) {
				output += (uint)((255 - (uint)(byteArray[offset - i])) * Math.Pow(2, (8 * i)));
			}
			return ++output;
        }

        private static DateTime BytesToDateTime (byte[] byteArray, int offset)
        {
			uint number = BytesToUInt(byteArray, offset);
			return STARTOFTIME.AddSeconds(number);
	    }
	}
}
