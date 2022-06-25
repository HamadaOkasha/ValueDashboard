using App.Application.Caching;
using App.Application.Framwork;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.Services;
using App.Application.Validator;
using App.Domain.Interfaces;
using App.Infra.Data.Repositories;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace App.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Repository
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IValueRepository, ValueRepository>();
            services.AddScoped<IResearchValueRepository, ResearchValueRepository>();
            services.AddScoped<IAlrowadVersionRepository, AlrowadVersionRepository>();
            services.AddScoped<IAlrowadDataRepository, AlrowadDataRepository>();

            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IAdminPagesRepository, AdminPagesRepository>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessagesTemplateRepository, MessagesTemplateRepository>();
            services.AddScoped<ILocaleStringResourceRepository, LocaleStringResourceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGeneralSettingRepository, GeneralSettingRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            //Sevices
            services.AddScoped<ICountrySevices, CountrySevices>();
            services.AddScoped<ISectorSevices, SectorSevices>();
            services.AddScoped<IOrganizationSevices, OrganizationSevices>();
            services.AddScoped<ICategorySevices, CategorySevices>();
            services.AddScoped<IValueSevices, ValueSevices>();
            services.AddScoped<IResearchValueSevices, ResearchValueSevices>();
            services.AddScoped<IAlrowadVersionSevices, AlrowadVersionSevices>();
            services.AddScoped<IAlrowadDataSevices, AlrowadDataSevices>();

            services.AddScoped<IGeneralSettingSevices, GeneralSettingSevices>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ILanguageSevices, LanguageSevices>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IAdminPagesSevices, AdminPagesSevices>();
            services.AddScoped<ILocalizedModelFactory, LocalizedModelFactory>();
            services.AddScoped<ILookupSevices, LookupSevices>();
            services.AddScoped<IInitalizeModelLookups, InitalizeModelLookups>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IMessagesTemplateServices, MessagesTemplateServices>();
            services.AddScoped<ILocaleStringResourceSevices, LocaleStringResourceSevices>();
            services.AddScoped<IUsersService, UsersService>();
 
            //Validator
            services.AddTransient<IValidator<LanguageModel>, LanguageValidator>();
            services.AddTransient<IValidator<AlrowadDataModel>, AlrowadDataValidator>();
            services.AddTransient<IValidator<LocaleStringResourceModel>, LocaleStringResourceValidator>();
            services.AddScoped<ICacheManager, MemoryCacheManager>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
