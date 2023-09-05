using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Statistics.Dto;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Exporting
{
    public interface IReportExcelExporter
    {
        //FileDto ExportToFile(List<ListDto> auditLogListDtos);
        Task<FileDto> ExportBM01ToFile(List<BM01Dto> bm01Dtos, DateTime fromDate, DateTime toDate, string orgName);
        Task<FileDto> ExportBM01aToFile(List<BM01aDto> bm01Dtos, int year, string orgName);
        Task<FileDto> ExportBM02ToFile(List<Bm02Dto> bm02Dtos, DateTime fromDate, DateTime toDate, string org);
        Task<FileDto> ExportBM04ToFile(List<BM04Dto> bm04ListDto, DateTime fromDate, DateTime toDate, string org);
        Task<FileDto> ExportBM03ToFile(List<BM03Dto> bm03Dto, DateTime fromDate, DateTime toDate, string orgName);
        Task<FileDto> ExportBM05ToFile(List<BM05Dto> bm05Dto, DateTime fromDate, DateTime toDate, string orgName);
        Task<FileDto> ExportBM07ToFile(List<BM07Dto> bM07Dto, DateTime fromDate, DateTime toDate, string org);
        Task<FileDto> ExportBM08ToFile(List<BM08Dto> bM08Dto, DateTime fromDate, DateTime toDate, string org);
        Task<FileDto> ExportBM09ToFile(List<BM09Dto> bm05Dto, int fromDate, int toDate, string orgName);
        Task<FileDto> ExportBM10ToFile(List<BM10Dto> bM10Dto,int count1, int count2, int count3, string org);
        Task<FileDto> ExportBM12ToFile(List<BM12Dto> bM12Dto, DateTime fromDate, DateTime toDate, string orgName);
        Task<FileDto> ExportBM13ToFile(List<BM13Dto> bM13Dto, DateTime fromDate, DateTime toDate, string org);
        Task<FileDto> ExportBM15ToFile(List<BM15Dto> bm15Dto, string orgName);
        Task<FileDto> ExportHS02(ExportHS02Dto person);
        Task<FileDto> ExportStatisticToFile(List<StatisticDto> staffs);
    }
}