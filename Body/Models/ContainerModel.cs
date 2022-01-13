using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class ContainerModel
    {
        private readonly short MaxSlotsCount;
        public int GetSlotsCount => MaxSlotsCount;
        public string SufixName { get; set; }
        public bool IsEmpty { get; set; }
        private ProductModel[] Slots { get; set; }
        public ProductModel[] GetSlots => Slots;
        public ProductModel GetSlot(int index) => index >= 0 && index < MaxSlotsCount ? Slots[index] : null;
        public void SetSlot(int index, ProductModel product)
        {
            if(index >= 0 && index < MaxSlotsCount) Slots[index] = product;
            IsEmpty = true;
            foreach (var p in Slots) if (p != null) { IsEmpty = false; break; }
        }
        
        public ContainerModel(string[] _ProductAmount, short _MaxSlots, string _SufixName)
        {
            SufixName = _SufixName;
            MaxSlotsCount = _MaxSlots;
            Slots = new ProductModel[MaxSlotsCount];
            IsEmpty = true;
            int index = 0;
            while (index < GetSlotsCount && index < _ProductAmount.Length)
            {
                string[] ProductInfo = _ProductAmount[index].Split(':');
                var product = ObjectModel.GetObject(ProductInfo[0]).ToProduct();
                product.Amount = int.Parse(ProductInfo[1]);
                SetSlot(index, product);
                index++;
            }
            foreach (var p in Slots) if (p != null) { IsEmpty = false; break; }
        }
        public ContainerModel(ProductModel[] _Products, short _MaxSlots)
        {
            SufixName = "";
            MaxSlotsCount = _MaxSlots;
            Slots = new ProductModel[MaxSlotsCount];
            IsEmpty = true;
            int index = 0;
            while (index < GetSlotsCount && index < _Products.Length)
            {
                SetSlot(index, _Products[index].ToProduct());
                index++;
            }
            foreach (var p in Slots) if (p != null) { IsEmpty = false; break; }
        }
        public ContainerModel(List<ProductModel> _Products, short _MaxSlots)
        {
            SufixName = "";
            MaxSlotsCount = _MaxSlots;
            Slots = new ProductModel[MaxSlotsCount];
            IsEmpty = true;
            int index = 0;
            while (index < GetSlotsCount && index < _Products.Count)
            {
                SetSlot(index, _Products[index].ToProduct());
                index++;
            }
            foreach (var p in Slots) if (p != null) { IsEmpty = false; break; }
        }
        public ContainerModel(string _PocketString, short _MaxSlots, string _SufixName)
        {
            SufixName = _SufixName;
            MaxSlotsCount = _MaxSlots;
            Slots = new ProductModel[MaxSlotsCount];
            IsEmpty = true;
            int index = 0;
            while (index < GetSlotsCount && index * 3 < _PocketString.Length)
            {
                SetSlot(index, new ProductModel(_PocketString.Substring(index * 3, 3)).ToProduct());
                index++;
            }
            foreach (var p in Slots) if (p != null) { IsEmpty = false; break; }
        }
        public override string ToString()
        {
            string _PocketString = "";
            foreach (var slot in Slots) if (slot != null) _PocketString += slot.ToString();
            return _PocketString;
        }
    }
}
