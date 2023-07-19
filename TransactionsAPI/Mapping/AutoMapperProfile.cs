using AutoMapper;
using TransactionsAPI.Database.Entities;
using TransactionsAPI.Models;

namespace TransactionsAPI.Mapping {
    public class AutoMapperProfile : Profile{
        public AutoMapperProfile() {

            CreateMap<Category, CategoryEntity>();

            CreateMap<CategoryEntity, Category>();

            CreateMap<TransactionEntity, Transaction>();

            CreateMap<Transaction, TransactionEntity>();

            CreateMap<PageSortedList<TransactionEntity>, PageSortedList<Transaction>>();

            CreateMap<PageSortedList<Transaction>, PageSortedList<TransactionEntity>>();

        }
    }
}
