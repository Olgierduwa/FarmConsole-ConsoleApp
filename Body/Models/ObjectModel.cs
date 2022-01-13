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
        public int Category { get; set; }
        public int Scale { get; set; }
        public int Type { get; set; }
        private int _State { get; set; }
        public int State { get => _State; set { _State = value; Edited = true; } }
        public short ID { get; set; }
        private bool Edited { get; set; }
        public string StateName { get; set; }
        public string ObjectName { get; set; }
        public string Property { get; set; }
        public int Price { get; set; }
        public string[] MenuActions { get; set; }
        public string[] MapActions { get; set; }
        public bool Cutted { get; set; }
        public short Slots { get; set; }
        public ViewModel View { get; set; }
        #endregion

        private void SetID()
        {
            short Index = 0;
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
            Edited = false;
        }
        private ObjectModel Clone(object ExpectedModel)
        {
            ObjectModel O = new ObjectModel();
            if(ExpectedModel is ProductModel) O = new ProductModel();
            if(ExpectedModel is FieldModel) O = new FieldModel();
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
            O.Slots = Slots;
            O.View = View.ViewClone();
            return O;
        }
        public ProductModel ToProduct(int amount = -1)
        {
            string BaseAction = Objects[ID].MapActions.Length > 0 ? Objects[ID].MapActions[0] : null;
            if (Edited) SetID();
            ProductModel NewProduct = (ProductModel)Objects[ID].Clone(new ProductModel());
            string CurrentAcction = MapActions != null ? MapActions.Length > 0 ? MapActions[0] : null : null;
            NewProduct.ID = ID;
            NewProduct.Amount = 1;
            NewProduct.MapActions = BaseAction != CurrentAcction ? MapActions : NewProduct.MapActions;
            if (amount < 0)
            {
                ObjectModel ThisObject = this;
                if (ThisObject is ProductModel) NewProduct.Amount = ((ProductModel)ThisObject).Amount;
            }
            else NewProduct.Amount = amount;
            return NewProduct;
        }
        public FieldModel ToField()
        {
            string BaseAction = Objects[ID].MapActions.Length > 0 ? Objects[ID].MapActions[0] : null;
            if (Edited) SetID();
            FieldModel NewField = (FieldModel)Objects[ID].Clone(new FieldModel());
            string CurrentAction = MapActions != null ? MapActions.Length > 0 ? MapActions[0] : null : null;
            NewField.ID = ID;
            if (NewField.Slots > 0) NewField.Pocket = new ContainerModel(new ProductModel[0], NewField.Slots);
            ObjectModel ThisObject = this;
            NewField.MapActions = BaseAction != CurrentAction ? MapActions : NewField.MapActions;
            if (ThisObject is FieldModel)
            {
                if (((FieldModel)ThisObject).New == false)
                {
                    if (((FieldModel)ThisObject).BaseView != null) NewField.BaseView = ((FieldModel)ThisObject).BaseView.ViewClone();
                    //if (((FieldModel)this).View != null) NewField.View = ((FieldModel)this).View.ViewClone();
                    NewField.Duration = ((FieldModel)ThisObject).Duration;
                    NewField.BaseID = ((FieldModel)ThisObject).BaseID;
                    NewField.Pocket = ((FieldModel)ThisObject).Pocket;
                    NewField.ArrivalDirection = ((FieldModel)ThisObject).ArrivalDirection;
                }
                else ((FieldModel)ThisObject).New = false;
            }
            return NewField;
        }
        public override string ToString()
        {
            return StateName + " " + ObjectName;
        }

        private static List<ObjectModel> Objects = XF.GetObjects();
        public static void SetObjects() => Objects = XF.GetObjects();
        public static ObjectModel GetObject(int _ID) => Objects[_ID];
        public static ObjectModel GetObject(string _Name, int _State = 0, string _StateName = "", string sufix = "")
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
            if (Objects[Index].Property == "") Objects[Index].Property = sufix;
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
