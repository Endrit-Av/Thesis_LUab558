﻿using AutoMapper;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Entity;

namespace Thesis_LUab558.Server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Cart, CartDto>();
            CreateMap<Image, ImageDto>();
            CreateMap<InvoiceDetail, InvoiceDetailDto>();
            CreateMap<InvoiceItem, InvoiceItemDto>();
            CreateMap<Review, ReviewDto>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName));
            CreateMap<User, UserDto>();
            CreateMap<Wishlist, WishlistDto>();
        }
    }
}
