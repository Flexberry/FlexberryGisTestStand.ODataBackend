﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IIS.FlexberryGisTestStand
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// Author.
    /// </summary>
    // *** Start programmer edit section *** (Author CustomAttributes)

    // *** End programmer edit section *** (Author CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("AuthorE", new string[] {
            "Name as \'Name\'",
            "Phone as \'Phone\'",
            "Email as \'Email\'",
            "Birthday as \'Birthday\'",
            "Gender as \'Gender\'",
            "Vip as \'Vip\'"})]
    [View("AuthorL", new string[] {
            "Name as \'Name\'",
            "Phone as \'Phone\'",
            "Email as \'Email\'",
            "Birthday as \'Birthday\'",
            "Gender as \'Gender\'",
            "Vip as \'Vip\'"})]
    public class Author : ICSSoft.STORMNET.DataObject
    {
        
        private string fName;
        
        private long fPhone;
        
        private string fEmail;
        
        private ICSSoft.STORMNET.UserDataTypes.NullableDateTime fBirthday;
        
        private IIS.FlexberryGisTestStand.tGender fGender;
        
        private bool fVip;
        
        // *** Start programmer edit section *** (Author CustomMembers)

        // *** End programmer edit section *** (Author CustomMembers)

        
        /// <summary>
        /// Name.
        /// </summary>
        // *** Start programmer edit section *** (Author.Name CustomAttributes)

        // *** End programmer edit section *** (Author.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (Author.Name Get start)

                // *** End programmer edit section *** (Author.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (Author.Name Get end)

                // *** End programmer edit section *** (Author.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Author.Name Set start)

                // *** End programmer edit section *** (Author.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (Author.Name Set end)

                // *** End programmer edit section *** (Author.Name Set end)
            }
        }
        
        /// <summary>
        /// Phone.
        /// </summary>
        // *** Start programmer edit section *** (Author.Phone CustomAttributes)

        // *** End programmer edit section *** (Author.Phone CustomAttributes)
        public virtual long Phone
        {
            get
            {
                // *** Start programmer edit section *** (Author.Phone Get start)

                // *** End programmer edit section *** (Author.Phone Get start)
                long result = this.fPhone;
                // *** Start programmer edit section *** (Author.Phone Get end)

                // *** End programmer edit section *** (Author.Phone Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Author.Phone Set start)

                // *** End programmer edit section *** (Author.Phone Set start)
                this.fPhone = value;
                // *** Start programmer edit section *** (Author.Phone Set end)

                // *** End programmer edit section *** (Author.Phone Set end)
            }
        }
        
        /// <summary>
        /// Email.
        /// </summary>
        // *** Start programmer edit section *** (Author.Email CustomAttributes)

        // *** End programmer edit section *** (Author.Email CustomAttributes)
        [StrLen(255)]
        public virtual string Email
        {
            get
            {
                // *** Start programmer edit section *** (Author.Email Get start)

                // *** End programmer edit section *** (Author.Email Get start)
                string result = this.fEmail;
                // *** Start programmer edit section *** (Author.Email Get end)

                // *** End programmer edit section *** (Author.Email Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Author.Email Set start)

                // *** End programmer edit section *** (Author.Email Set start)
                this.fEmail = value;
                // *** Start programmer edit section *** (Author.Email Set end)

                // *** End programmer edit section *** (Author.Email Set end)
            }
        }
        
        /// <summary>
        /// Birthday.
        /// </summary>
        // *** Start programmer edit section *** (Author.Birthday CustomAttributes)

        // *** End programmer edit section *** (Author.Birthday CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.NullableDateTime Birthday
        {
            get
            {
                // *** Start programmer edit section *** (Author.Birthday Get start)

                // *** End programmer edit section *** (Author.Birthday Get start)
                ICSSoft.STORMNET.UserDataTypes.NullableDateTime result = this.fBirthday;
                // *** Start programmer edit section *** (Author.Birthday Get end)

                // *** End programmer edit section *** (Author.Birthday Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Author.Birthday Set start)

                // *** End programmer edit section *** (Author.Birthday Set start)
                this.fBirthday = value;
                // *** Start programmer edit section *** (Author.Birthday Set end)

                // *** End programmer edit section *** (Author.Birthday Set end)
            }
        }
        
        /// <summary>
        /// Gender.
        /// </summary>
        // *** Start programmer edit section *** (Author.Gender CustomAttributes)

        // *** End programmer edit section *** (Author.Gender CustomAttributes)
        public virtual IIS.FlexberryGisTestStand.tGender Gender
        {
            get
            {
                // *** Start programmer edit section *** (Author.Gender Get start)

                // *** End programmer edit section *** (Author.Gender Get start)
                IIS.FlexberryGisTestStand.tGender result = this.fGender;
                // *** Start programmer edit section *** (Author.Gender Get end)

                // *** End programmer edit section *** (Author.Gender Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Author.Gender Set start)

                // *** End programmer edit section *** (Author.Gender Set start)
                this.fGender = value;
                // *** Start programmer edit section *** (Author.Gender Set end)

                // *** End programmer edit section *** (Author.Gender Set end)
            }
        }
        
        /// <summary>
        /// Vip.
        /// </summary>
        // *** Start programmer edit section *** (Author.Vip CustomAttributes)

        // *** End programmer edit section *** (Author.Vip CustomAttributes)
        public virtual bool Vip
        {
            get
            {
                // *** Start programmer edit section *** (Author.Vip Get start)

                // *** End programmer edit section *** (Author.Vip Get start)
                bool result = this.fVip;
                // *** Start programmer edit section *** (Author.Vip Get end)

                // *** End programmer edit section *** (Author.Vip Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Author.Vip Set start)

                // *** End programmer edit section *** (Author.Vip Set start)
                this.fVip = value;
                // *** Start programmer edit section *** (Author.Vip Set end)

                // *** End programmer edit section *** (Author.Vip Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "AuthorE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View AuthorE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("AuthorE", typeof(IIS.FlexberryGisTestStand.Author));
                }
            }
            
            /// <summary>
            /// "AuthorL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View AuthorL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("AuthorL", typeof(IIS.FlexberryGisTestStand.Author));
                }
            }
        }
    }
}
