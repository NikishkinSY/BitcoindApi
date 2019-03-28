using System;
using AutoMapper;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.DAL.Entities;

namespace Bitcoind.Core.Automapper
{
    public class CategoryResolver : IValueResolver<BitcoinTransactionDto, Transaction, Category>
    {
        public Category Resolve(BitcoinTransactionDto source, Transaction dest, Category destMember, ResolutionContext context)
        {
            return Enum.TryParse(source.Category, true, out Category category)
                ? category
                : Category.Unknown;
        }
    }
}
