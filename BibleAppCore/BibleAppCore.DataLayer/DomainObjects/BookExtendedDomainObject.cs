﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BibleAppCore.Contracts.Contract.ViewModel;


namespace BibliaApp
{
    [Table("BookExtended")]
    internal class BookExtendedDomainObject
    {
        public Guid Guid { get; set; }
        public string BookName { get; set; }
        public string BookFullName { get; set; }
        public string PassagesJson { get; set; }
        public int SubbookNumber { get; set; }
        public int BookGlobalNumber { get; set; }

        public Guid NextBookGuid { get; set; }
        public Guid PreviousBookGuid { get; set; }

        [NotMapped]
        public List<PassageDomainObject> Passages { get; set; }

        [NotMapped]
        public List<CommentDomainObject> Comments { get; set; }


        public void BeforeSave()
        {
            PassagesJson = JsonConvert.SerializeObject(Passages);

        }

        public void OnRead()
        {
            if (PassagesJson != null)
            {
                Passages = JsonConvert.DeserializeObject<List<PassageDomainObject>>(PassagesJson);
                PassagesJson = null;
            }

        }

    }

}