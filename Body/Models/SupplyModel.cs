using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class SupplyModel
    {
        private int MapSize { get; set; }
        private List<int> DeliveryDays { get; set; }
        private ContainerModel[] Containers { get; set; }
        private Point[] ContainersPos { get; set; }
        private int GetContainerIndex(Point ContainerPos)
        {
            int ContainerIndex = 0;
            while (ContainerIndex < ContainersPos.Length && ContainersPos[ContainerIndex] != ContainerPos) ContainerIndex++;
            if (ContainerIndex == ContainersPos.Length) return -1;
            else return ContainerIndex;
        }
        private ProductModel GetSlotFromContainer(Point ContainerPos, int SlotIndex)
        {
            int ContainerIndex = GetContainerIndex(ContainerPos);
            if (ContainerIndex < 0) return null;
            return Containers[ContainerIndex].GetSlot(SlotIndex);
        }

        public SupplyModel(ContainerModel[] _containers, Point[] _containerspos, List<int> _deliverydays, int _mapsize)
        {
            MapSize = _mapsize;
            Containers = _containers;
            ContainersPos = _containerspos;
            DeliveryDays = _deliverydays;
        }
        public SupplyModel(string SupplyString, int _MapSize)
        {
            MapSize = _MapSize;
            string[] DeliveryDaysString = SupplyString.Split(':')[0].Split(',');
            string[] ContainersIDS = SupplyString.Split(':')[1].Split('/');
            string[] ContainersContent = SupplyString.Split(':')[2].Split('/');
            int Count = ContainersIDS.Length;
            ContainersPos = new Point[Count];
            Containers = new ContainerModel[Count];
            for (int i = 0; i < Count; i++)
            {
                int pos = int.Parse(ContainersIDS[i]);
                string ProductsString = ContainersContent[i];
                var Products = new ProductModel[ProductsString.Length / 3];
                for (int j = 0; j < ProductsString.Length; j += 3)
                    Products[j / 3] = new ProductModel(ProductsString.Substring(j, 3)).ToProduct();

                ContainersPos[i] = new Point(pos / _MapSize, pos % _MapSize);
                Containers[i] = new ContainerModel(Products, Convert.ToInt16(Products.Length));
            }
            DeliveryDays = new List<int>();
            foreach (var day in DeliveryDaysString) DeliveryDays.Add(int.Parse(day));
        }
        public override string ToString()
        {
            string ContainersIDS = "", ContainersContent = "", DeliveryDaysString = "";
            for (int i = 0; i < Containers.Length; i++)
            {
                ContainersIDS += (ContainersPos[i].X * MapSize + ContainersPos[i].Y).ToString() + "/";
                ContainersContent += Containers[i].ToString() + "/";
            }
            foreach (var day in DeliveryDays) DeliveryDaysString += day.ToString() + ",";
            DeliveryDaysString = DeliveryDaysString[0..^1];
            ContainersIDS = ContainersIDS.Length == 0 ? "-" : ContainersIDS[0..^1];
            ContainersContent = ContainersContent.Length == 0 ? "-" : ContainersContent[0..^1];
            return DeliveryDaysString + ":" + ContainersIDS + ":" + ContainersContent;
        }
        public void SortContainers(FieldModel[,] Fields, List<ProductModel> FoundProducts)
        {
            for (int x = 1; x < MapSize; x++)
                for (int y = 1; y < MapSize; y++)
                {
                    var pocket = Fields[x, y].Pocket;
                    if (pocket != null)
                    {
                        for (int i = 0; i < pocket.GetSlotsCount; i++)
                        {
                            var PocketSlot = pocket.GetSlot(i);
                            var SupplySlot = GetSlotFromContainer(new Point(x, y), i);
                            if (PocketSlot != null && (SupplySlot == null || PocketSlot.ObjectName != SupplySlot.ObjectName))
                            {
                                FoundProducts.Add(PocketSlot);
                                pocket.SetSlot(i, null);
                            }
                        }
                    }
                }

            if (FoundProducts.Count > 0)
                for (int i = 0; i < ContainersPos.Length; i++)
                {
                    var SupplyPos = ContainersPos[i];
                    var SupplyContainer = Containers[i];
                    for (int j = 0; j < SupplyContainer.GetSlotsCount; j++)
                    {
                        var SupplyContainerSlot = SupplyContainer.GetSlot(j);
                        var Product = FoundProducts.Find(p => p.ObjectName == SupplyContainerSlot.ObjectName);
                        if (Product != null)
                        {
                            var pocket = Fields[SupplyPos.X, SupplyPos.Y].Pocket;
                            if (pocket.GetSlot(j) == null) pocket.SetSlot(j, Product);
                            else pocket.GetSlot(j).Amount += Product.Amount;
                            FoundProducts.Remove(Product);
                        }
                    }
                }
        }
        public void Delivery(FieldModel[,] Fields, DateTime NowDate, DateTime LastDate)
        {
            bool delivery = false;
            if (NowDate.Month > LastDate.Month || NowDate.Year > LastDate.Year) delivery = true;
            else if(NowDate != LastDate) foreach (int day in DeliveryDays)
                    if (NowDate.Day >= day && LastDate.Day < day) { delivery = true; break; }

            if (delivery)
            {
                for (int i = 0; i < ContainersPos.Length; i++)
                {
                    var SupplyPos = ContainersPos[i];
                    var SupplyContainer = Containers[i];
                    var pocket = Fields[SupplyPos.X, SupplyPos.Y].Pocket;
                    for (int j = 0; j < SupplyContainer.GetSlotsCount; j++)
                        pocket.SetSlot(j, SupplyContainer.GetSlot(j)?.ToProduct());
                }
            }
        }
        public List<ProductModel> GetSupplyProducts()
        {
            List<ProductModel> Products = new List<ProductModel>();
            foreach (var c in Containers)
                foreach (var s in c.GetSlots)
                    if (s != null) Products.Add(s);
            return Products;
        }
    }
}
