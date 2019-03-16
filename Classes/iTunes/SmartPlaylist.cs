using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace if2ktool.iTunes
{
    public class SmartPlaylist
    {
        // Coincides with "Match" checkbox
        public bool match;

        // Coincides with the "Match" dropdown (Any/All)
        public MatchOperator matchOperator;

        // Coincides with the "Limit" checkbox and the options next to it
        // e.g. Limit to <limitNumber> <limitMethod>...
        public bool limit;
        public int limitNumber;
        public LimitMethod limitMethod;

        // Coincides with the "selected by" dropdown next to the limit options
        public SelectionMethod selectionMethod;
        public LogicSign selectionMethodSign;

        // Coincides with the "Match only checked items" checkbox. Has no effect in foobar
        public bool matchOnlyChecked;

        // Coincides with the "Live updating checkbox". Has no effect in foobar
        public bool liveUpdating;

        // Array of rules to match
        public Rule[] rules;

        public class Rule
        {
            // The field to apply the condition to
            public Field field;
            public FieldType fieldType;

            // The condition to apply
            public LogicRule logicRule;

            // The logical sign of the condition
            public LogicSign logicSign = LogicSign.None;

            // A flag to specify which mode the LogicRule.Other is in
            // This has different implications depending on the field type
            public int logicRuleOtherMode;

            // Data
            public string stringData;
            public int integerA, integerB;
            public bool booleanData;
            public uint timestampA, timestampB;
            public DateFieldType dateFieldType;

            public MediaKind mediaKindData;

            // 
            public Rule[] subrules;
            public MatchOperator subruleMatchOperator;
        }
    }
}
