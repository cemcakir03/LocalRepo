using CoreBLOG.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBLOG.MODEL.Entities
{
    public class Comment : CoreEntity
    {
        public string CommentText { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
