using AutoMapper;
using quanlykytuc.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using quanlykytuc.ViewModels;

namespace quanlykytuc.Helpers
{
    public class AutoMaperProfile: Profile
    {
        public AutoMaperProfile()
        {
            CreateMap<RegisterVM, User>();
            //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
            //.ReverseMap();
        }
    }
}

