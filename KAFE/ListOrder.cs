//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KAFE
{
    using System;
    using System.Collections.Generic;
    
    public partial class ListOrder
    {
        public int ID_ListOrder { get; set; }
        public int ID_Order { get; set; }
        public int ID_Food { get; set; }
        public int Quantity { get; set; }
    
        public virtual Food Food { get; set; }
        public virtual Orders Orders { get; set; }
    }
}
