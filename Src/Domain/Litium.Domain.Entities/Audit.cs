//using System;
//using Litium.Domain.Entities.Customers;
//using Litium.Engine.Domain;

//namespace Litium.Domain.Entities
//{
//    public class Audit
//    {
//        private Person _createdBy;
//        private Person _updatedBy;

//        public DateTime Created { get; protected internal set; }
//        public Person CreatedBy
//        {
//            get { return _createdBy; }
//            protected internal set
//            {
//                _createdBy = value;
//                SetGuid(this, x => x.CreatedById, value);
//            }
//        }

//        public Guid CreatedById { get; protected internal set; }
//        public DateTime Updated { get; protected internal set; }
//        public Person UpdatedBy
//        {
//            get { return _updatedBy; }
//            protected internal set
//            {
//                _updatedBy = value;
//                SetGuid(this, x => x.UpdatedById, value);
//            }
//        }

//        public Guid UpdatedById { get; protected internal set; }
//    }
//}

