using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QPMS.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmployeeStart : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public EmployeeStart(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        FileData marriageCertificate;
        FileData diploma;
        FileData criminalRecords;
        FileData identityCardCopy;
        FileData certificateOfResidence;
        FileData healthReport;
        string address;
        string zip;
        string city;
        string country;
        string emailPersonal;
        string mobile;
        string citizenNumber;
        string surname;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Surname
        {
            get => surname;
            set => SetPropertyValue(nameof(Surname), ref surname, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CitizenNumber
        {
            get => citizenNumber;
            set => SetPropertyValue(nameof(CitizenNumber), ref citizenNumber, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Mobile
        {
            get => mobile;
            set => SetPropertyValue(nameof(Mobile), ref mobile, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EmailPersonal
        {
            get => emailPersonal;
            set => SetPropertyValue(nameof(EmailPersonal), ref emailPersonal, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Cr
        {
            get => city;
            set => SetPropertyValue(nameof(Cr), ref city, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Zip
        {
            get => zip;
            set => SetPropertyValue(nameof(Zip), ref zip, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }

        
        public FileData MarriageCertificate
        {
            get => marriageCertificate;
            set => SetPropertyValue(nameof(MarriageCertificate), ref marriageCertificate, value);
        }



        public FileData HealthReport
        {
            get => healthReport;
            set => SetPropertyValue(nameof(HealthReport), ref healthReport, value);
        }


        public FileData CertificateOfResidence
        {
            get => certificateOfResidence;
            set => SetPropertyValue(nameof(CertificateOfResidence), ref certificateOfResidence, value);
        }


        public FileData IdentityCardCopy
        {
            get => identityCardCopy;
            set => SetPropertyValue(nameof(IdentityCardCopy), ref identityCardCopy, value);
        }


        public FileData CriminalRecords
        {
            get => criminalRecords;
            set => SetPropertyValue(nameof(CriminalRecords), ref criminalRecords, value);
        }

        
        public FileData Diploma
        {
            get => diploma;
            set => SetPropertyValue(nameof(Diploma), ref diploma, value);
        }
    }
}