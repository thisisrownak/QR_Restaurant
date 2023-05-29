using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Business.Abstract
{
    public interface IQrOrderTableService
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void Add(QrOrderTable entity);
        void Update(QrOrderTable entity);
        void SoftDelete(QrOrderTable entity);
        void HardDelete(int id);
        QrOrderTable GetQrOrderTable(int id);
        IEnumerable<QrOrderTable> GetAll();

        IEnumerable<QrOrderTable> GetQrTablesWithRestaurant(int restaurantId);

        IEnumerable<QrOrderTable> GetQrTablesWithActiveOrders(int restaurantId);
        QrOrderTable GetQrOrderTableWithOpenOrders(int id);

        QrOrderTable GetQrOrderTableByNoOrNameForRestaurant(int restaurantId, int? tableNo);
    }
}
