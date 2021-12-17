using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class ObjectModel
    {
        #region DEFINITIONS

        public int ID { get; set; }
        public int Category { get; set; }
        public int Scale { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public string StateName { get; set; }
        public string ObjectName { get; set; }
        public string Property { get; set; }
        public decimal Price { get; set; }
        public string[] MenuActions { get; set; }
        public string[] MapActions { get; set; }
        public bool Cutted { get; set; }
        public ViewModel View { get; set; }
        #endregion


        public void SetID()
        {
            int Index = 0;
            while (Index < Objects.Count)
            {
                if (Objects[Index].Category == Category &&
                    Objects[Index].Scale == Scale &&
                    Objects[Index].Type == Type &&
                    Objects[Index].State == State) break;
                Index++;
            }
            if (Index == Objects.Count) ID = 3; // error
            else ID = Index;
        }
        private ObjectModel Clone(object type)
        {
            ObjectModel O = new ObjectModel();
            if(type is ProductModel) O = new ProductModel();
            if(type is FieldModel) O = new FieldModel();
            O.Category = Category;
            O.Scale = Scale;
            O.Type = Type;
            O.State = State;
            O.StateName = StateName;
            O.ObjectName = ObjectName;
            O.Property = Property;
            O.Price = Price;
            O.MenuActions = (string[])MenuActions.Clone();
            O.MapActions = (string[])MapActions.Clone();
            O.Cutted = Cutted;
            O.View = View.ViewClone();
            return O;
        }
        public ProductModel ToProduct()
        {
            ProductModel NewProduct = (ProductModel)Objects[ID].Clone(new ProductModel());
            NewProduct.ID = ID;
            NewProduct.Amount = 1;
            if (this is ProductModel)
            {
                NewProduct.Amount = ((ProductModel)this).Amount; 
            }
            return NewProduct;
        }
        public FieldModel ToField()
        {
            FieldModel NewField = (FieldModel)Objects[ID].Clone(new FieldModel());
            NewField.ID = ID;
            if (this is FieldModel)
            {
                if(((FieldModel)this).BaseView != null)
                    NewField.BaseView = ((FieldModel)this).BaseView.ViewClone();
                NewField.Duration = ((FieldModel)this).Duration;
                NewField.BaseID = ((FieldModel)this).BaseID;
                NewField.ArrivalDirection = ((FieldModel)this).ArrivalDirection;
            }
            return NewField;
        }
        public override string ToString()
        {
            return ObjectName;
        }


        private static List<ObjectModel> Objects = XF.GetObjects();
        public static ObjectModel GetObject(int _ID) => Objects[_ID];
        public static ObjectModel GetObject(string _Name, int _State = 0, string _StateName = "")
        {
            int Index = 0;
            while (Index < Objects.Count)
            {
                if (Objects[Index].ObjectName == _Name &&
                  ((_StateName != "" && Objects[Index].StateName == _StateName) ||
                   (_StateName == "" && Objects[Index].State == _State))) break;
                Index++;
            }
            if (Index == Objects.Count) return Objects[3]; // error
            return Objects[Index];
        }
        public static List<ObjectModel> GetObjects(int? _Category = null, int? _Scale = null, int? _Type = null, int? _State = null)
        {
            List<ObjectModel> objects = new List<ObjectModel>();
            if (_Category != null || _Scale != null || _Type != null || _State != null)
                foreach (var p in Objects)
                {
                    if ((_Category == null || p.Category == _Category) &&
                        (_Scale == null || p.Scale == _Scale) &&
                        (_Type == null || p.Type == _Type) &&
                        (_State == null || p.State == _State))
                        objects.Add(p);
                }
            return objects;
        }
    }
}
