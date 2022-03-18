using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLibrary.Models
{
    /// <summary>
    /// Stipped down just to make sense of the demo
    /// </summary>
    public class ProjectModel
    {
        public int ProjectID { get; set; }
        /// <summary>
        /// Summary tasks to represent the project on the master prgramme of all projects
        /// and be the parentID for main project tasks
        /// </summary>
        public int SummaryTaskID { get; set; }

        [StringLength(50)]
        public string ProjectNumber { get; set; }
        [StringLength(50)]
        public string ProjectName { get; set; }
        [StringLength(50)]
        public string ProjectIdentifier { get; set; }
        /// <summary>
        /// ProjectNumber - Project Name (Project Identifier)
        /// </summary>
        public string ProjectNumberNameIdentifier => $"{ ProjectNumber } - { ProjectName } ({ ProjectIdentifier })";


        public DateTime? DateFeeProposealIssued { get; set; }
        public DateTime? DateContract { get; set; }
        public DateTime? DateSiteStart { get; set; }
        public DateTime? DateSiteEnd { get; set; }
        public DateTime? DateEndOfDefects { get; set; }
    }
}
