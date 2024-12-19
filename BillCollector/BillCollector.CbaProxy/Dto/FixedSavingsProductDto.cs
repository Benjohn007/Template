using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.CbaProxy.Dto
{
    public class FixedSavingsProductDto
    {
    }

    public class FixedSavingsProductsListDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string description { get; set; }
        public Currency currency { get; set; }
        public bool preClosurePenalApplicable { get; set; }
        public int minDepositTerm { get; set; }
        public int maxDepositTerm { get; set; }
        public Mindeposittermtype minDepositTermType { get; set; }
        public Maxdeposittermtype maxDepositTermType { get; set; }
        public int nominalAnnualInterestRate { get; set; }
        public Interestcompoundingperiodtype interestCompoundingPeriodType { get; set; }
        public Interestpostingperiodtype interestPostingPeriodType { get; set; }
        public Interestcalculationtype interestCalculationType { get; set; }
        public Interestcalculationdaysinyeartype interestCalculationDaysInYearType { get; set; }
        public Accountingrule accountingRule { get; set; }
        

        public class Currency
        {
            public string code { get; set; }
            public string name { get; set; }
            public int decimalPlaces { get; set; }
            public int inMultiplesOf { get; set; }
            public string displaySymbol { get; set; }
            public string nameCode { get; set; }
            public string displayLabel { get; set; }
        }

        public class Mindeposittermtype
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Maxdeposittermtype
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Interestcompoundingperiodtype
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Interestpostingperiodtype
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Interestcalculationtype
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Interestcalculationdaysinyeartype
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Accountingrule
        {
            public int id { get; set; }
            public string code { get; set; }
            public string value { get; set; }
        }

    }

}
