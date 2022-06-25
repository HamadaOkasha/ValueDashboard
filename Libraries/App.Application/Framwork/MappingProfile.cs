using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Model;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Framwork
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //AdminPages
            CreateMap<MessageTemplate, MessagesTemplateModel>();

            CreateMap<MessageTemplate, MessagesTemplateViewModel>();

            CreateMap<MessagesTemplateModel, MessageTemplate>(); 
            //AdminPages
            CreateMap<AdminPages, AdminPagesModel>();

            CreateMap<AdminPages, AdminPagesViewModel>();

            CreateMap<AdminPagesModel, AdminPages>();
            //GeneralSetting
            CreateMap<GeneralSetting, GeneralSettingModel>();

            CreateMap<GeneralSettingModel, GeneralSetting>();
            //AspNetRoles
            CreateMap<AspNetRoles, GroupModel>();
            CreateMap<AspNetRoles, GroupViewModel>();

            CreateMap<GroupModel, AspNetRoles>();

            //Language
            CreateMap<Language, LanguageModel>();

            CreateMap<Language, LanguageViewModel>();

            CreateMap<LanguageModel, Language>();


            //Country
            CreateMap<Country, CountryModel>();

            CreateMap<Country, CountryViewModel>();

            CreateMap<CountryModel, Country>();
            
            //Sector
            CreateMap<Sector, SectorModel>();

            CreateMap<Sector, SectorViewModel>();

            CreateMap<SectorModel, Sector>();

            //Organization
            CreateMap<Organization, OrganizationModel>();

            CreateMap<Organization, OrganizationViewModel>();

            CreateMap<OrganizationModel, Organization>();
            
            //Category
            CreateMap<Category, CategoryModel>();

            CreateMap<Category, CategoryViewModel>();

            CreateMap<CategoryModel, Category>();
            
            //Value
            CreateMap<Value, ValueModel>();

            CreateMap<Value, ValueViewModel>();

            CreateMap<ValueModel, Value>();
            
            //ResearchValue
            CreateMap<ResearchValue, ResearchValueModel>();

            CreateMap<ResearchValue, ResearchValueViewModel>();

            CreateMap<ResearchValueModel, ResearchValue>();

            //AlrowadVersion
            CreateMap<AlrowadVersion, AlrowadVersionModel>();

            CreateMap<AlrowadVersion, AlrowadVersionViewModel>();

            CreateMap<AlrowadVersionModel, AlrowadVersion>();

            //AlrowadData
            CreateMap<AlrowadData, AlrowadDataModel>();

            CreateMap<AlrowadData, AlrowadDataViewModel>();

            CreateMap<AlrowadDataModel, AlrowadData>();

            //// OrderStatus
            //CreateMap<OrderStatus, OrderStatusModel>();

            //CreateMap<OrderStatus, OrderStatusViewModel>();

            //CreateMap<OrderStatusModel, OrderStatus>();


            //LocaleStringResource
            CreateMap<LocaleStringResource, LocaleStringResourceModel>();

            CreateMap<LocaleStringResource, LocaleStringResourceViewModel>();

            CreateMap<LocaleStringResourceModel, LocaleStringResource>();

            // Notifications 
            CreateMap<Notification, NotificationModel>();
            CreateMap<NotificationModel, Notification>();
            CreateMap<Notification, NotificationViewModel>();

          
            //AspNetUsers
            CreateMap<UserModel, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, mo => mo.Ignore())
                .ForMember(dest => dest.SecurityStamp, mo => mo.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, mo => mo.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, mo => mo.Ignore())
                .ForMember(dest => dest.NormalizedEmail, mo => mo.Ignore())
                .ForMember(dest => dest.NormalizedUserName, mo => mo.Ignore())
                .ForMember(dest => dest.EmailConfirmed, mo => mo.Ignore())
                .ForMember(dest => dest.AccessFailedCount, mo => mo.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, mo => mo.Ignore());

            CreateMap<UserModelMobile, ApplicationUser>()
               .ForMember(dest => dest.PasswordHash, mo => mo.Ignore())
               .ForMember(dest => dest.SecurityStamp, mo => mo.Ignore())
               .ForMember(dest => dest.TwoFactorEnabled, mo => mo.Ignore())
               .ForMember(dest => dest.ConcurrencyStamp, mo => mo.Ignore())
               .ForMember(dest => dest.NormalizedEmail, mo => mo.Ignore())
               .ForMember(dest => dest.NormalizedUserName, mo => mo.Ignore())
               .ForMember(dest => dest.EmailConfirmed, mo => mo.Ignore())
               .ForMember(dest => dest.AccessFailedCount, mo => mo.Ignore())
               .ForMember(dest => dest.PhoneNumberConfirmed, mo => mo.Ignore());

            CreateMap<ApplicationUser, UserModel>()
               .ForMember(dest => dest.RoleName, mo => mo.Ignore())
               .ForMember(dest => dest.RolesList, mo => mo.Ignore())
               .ForMember(dest => dest.Password, mo => mo.Ignore())
               .ForMember(dest => dest.ConfirmPassword, mo => mo.Ignore());

            CreateMap<ApplicationUser, UserModelMobile>();


            CreateMap<AspNetUsers, UserViewModel>()
               .ForMember(dest => dest.RoleName, mo => mo.Ignore());

            //MessagesTemplate
            CreateMap<MessageTemplate, MessagesTemplateModel>();

            CreateMap<MessageTemplate, MessagesTemplateViewModel>();

            CreateMap<MessagesTemplateModel, MessageTemplate>();


            //MailSetting
            CreateMap<MailSetting, MailSettingModel>();

            CreateMap<MailSettingModel, MailSetting>();
        }
    }
}
