using Bitcoind.Core.DAL.Entities;
using System;
using AutoMapper;

namespace Bitcoind.Core.Automapper
{
    public class CategoryConverter: IValueConverter<string, Category>
    {
        public Category Convert(string source, ResolutionContext context)
        {
            return Enum.TryParse(source, true, out Category category)
                ? category
                : Category.Unknown;
        }
    }
}
