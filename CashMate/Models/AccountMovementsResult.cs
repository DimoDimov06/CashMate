namespace CashMate.Models
{
    using Google.Protobuf.WellKnownTypes;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Serialization;

    [XmlRoot("AccountMovementsResult")]
    public class AccountMovementsResult
    {
        [XmlElement("AccountMovementsFilter")]

        public AccountMovementsFilter Filter { get; set; }

        [XmlElement("Sums")]
        public Sums Sums { get; set; }

        [XmlElement("Bank")]
        public Bank Bank { get; set; }

        [XmlElement("Client")]
        public Client Client { get; set; }

        [XmlArray("AccountMovements")]
        [XmlArrayItem("AccountMovement")]
        public List<AccountMovement> AccountMovements { get; set; }
    }

    public class AccountMovementsFilter
    {
        public string BankAccountID { get; set; }

        [XmlIgnore]
        public DateTime StartDate { get; set; }
        [XmlElement("StartDate")]
        public string StartDateString
        {
            get => StartDate.ToString("yyyy-MM-dd");
            set => StartDate = DateTime.Parse(value);
        }


        [XmlIgnore] 
        public DateTime EndDate { get; set; }
        [XmlElement("EndDate")]
        public string EndDateString
        {
            get => EndDate.ToString("yyyy-MM-dd");
            set => EndDate = DateTime.Parse(value);
        }
    }

    public class Sums
    {
        [XmlIgnore]
        public decimal BeginSum { get; set; }

        [XmlElement("BeginSum")]
        public string BeginSumString
        {
            get { return BeginSum.ToString("0.##", CultureInfo.InvariantCulture); }
            set
            {
                // Заменяме запетаята с точка, за да можем да парсваме правилно
                var normalizedValue = value.Replace(",", ".");
                BeginSum = decimal.Parse(normalizedValue, CultureInfo.InvariantCulture);
            }
        }
        [XmlIgnore]
        public decimal TurnoverCR { get; set; }

        [XmlElement("TurnoverCR")]
        public string TurnoverCRString
        {
            get { return TurnoverCR.ToString("0.##", CultureInfo.InvariantCulture); }
            set
            {
                // Заменяме запетаята с точка, за да можем да парсваме правилно
                var normalizedValue = value.Replace(",", ".");
                TurnoverCR = decimal.Parse(normalizedValue, CultureInfo.InvariantCulture);
            }
        }
        [XmlIgnore]
        public decimal TurnoverDR { get; set; }

        [XmlElement("TurnoverDR")]
        public string TurnoverDRString
        {
            get { return TurnoverDR.ToString("0.##", CultureInfo.InvariantCulture); }
            set
            {
                // Заменяме запетаята с точка, за да можем да парсваме правилно
                var normalizedValue = value.Replace(",", ".");
                TurnoverDR = decimal.Parse(normalizedValue, CultureInfo.InvariantCulture);
            }
        }
        [XmlIgnore]
        public decimal EndSum { get; set; }

        [XmlElement("EndSum")]
        public string EndSumString
        {
            get { return EndSum.ToString("0.##", CultureInfo.InvariantCulture); }
            set
            {
                // Заменяме запетаята с точка, за да можем да парсваме правилно
                var normalizedValue = value.Replace(",", ".");
                EndSum = decimal.Parse(normalizedValue, CultureInfo.InvariantCulture);
            }
        }
    }

    public class Bank
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string BIC { get; set; }
    }

    public class Client
    {
        public string Name { get; set; }
        public string PersonalIdBulstat { get; set; }
        public string Address { get; set; }
    }

    public class AccountMovement
    {
        [XmlIgnore]
        public DateTime AccountingDate { get; set; }

        [XmlElement("AccountingDate")]
        public string AccountingDateString
        {
            get => AccountingDate.ToString("yyyy-MM-dd"); 
            set => AccountingDate = DateTime.Parse(value); 
        }

        [XmlIgnore]
        public DateTime ValueDate { get; set; }
        [XmlElement("ValueDate")]
        public string ValueDateString
        {
            get => ValueDate.ToString("yyyy-MM-dd");
            set => ValueDate = DateTime.Parse(value);
        }

        public string Reason { get; set; }
        public string OppositeSideName { get; set; }
        public string OppositeSideAccount { get; set; }
        public string MovementType { get; set; }

        [XmlIgnore]
        public string Amount { get; set; }

        [XmlElement("Amount")]
        public string AmountString
        {
            get { return Amount; }
            set
            {
                // Заменяме запетаята с точка, за да можем да парсваме правилно
                var normalizedValue = value.Replace(",", ".");
                Amount = normalizedValue;
            }
        }
        [XmlIgnore]
        public DateTime Date { get; set; }
        [XmlElement("Date")]
        public string DateString
        {
            get => Date.ToString("yyyy-MM-dd");
            set => Date = DateTime.Parse(value);
        }
        public string Hour { get; set; }
    }
}
