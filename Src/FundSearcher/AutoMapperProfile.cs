using AutoMapper;
using Fund.Crawler.Models;

namespace FundSearcher
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FundInfo, FundInfo>();
        }
    }
}
