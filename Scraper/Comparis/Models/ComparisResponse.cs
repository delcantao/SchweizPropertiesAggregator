namespace Scraper.Comparis.Models;

public class ComparisResponse
{
    public int[] AdIdList { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public ResultItems[] ResultItems { get; set; }
    public object PortalList { get; set; }
    public TargetingInformation TargetingInformation { get; set; }
    public PersistentTargetingInformation[] PersistentTargetingInformation { get; set; }
    public int NumberOfResults { get; set; }
    public string SearchParamsActivityGuid { get; set; }
    public CrossSellingLinks CrossSellingLinks { get; set; }
    public string ResultListUrl { get; set; }
    public MetaData MetaData { get; set; }
    public Breadcrumbs[] Breadcrumbs { get; set; }
    public string Title { get; set; }
    public object SimilarSearch { get; set; }
    public Header1 Header { get; set; }
}

public class ResultItems
{
    public int AdId { get; set; }
    public int SiteId { get; set; }
    public int AdStatus { get; set; }
    public string Title { get; set; }
    public string PropertyTypeText { get; set; }
    public string[] Address { get; set; }
    public string[] EssentialInformation { get; set; }
    public string Price { get; set; }
    public string Currency { get; set; }
    public string Date { get; set; }
    public string ImageUrl { get; set; }
    public string[] ImageUrls { get; set; }
    public bool IsPremiumListed { get; set; }
    public object ComparisPoints { get; set; }
    public Partners[] Partners { get; set; }
    public string PartnerLogoUrl { get; set; }
    public string PartnerName { get; set; }
    public ContactInformation ContactInformation { get; set; }
    public int? PriceDevelopmentDirection { get; set; }
    public int PriceValue { get; set; }
    public int? AreaValue { get; set; }
    public int PropertyTypeId { get; set; }
    public int DealType { get; set; }
    public bool UseInternalLinks { get; set; }
    public bool ShowComparisRating { get; set; }
    public object MemberLogoUrl { get; set; }
    public bool ShowDefaultPersonalizationSegment { get; set; }
    public string PriceTypeText { get; set; }
    public object Facts { get; set; }
    public object[] ViewTypes { get; set; }
}

public class Partners
{
    public string Name { get; set; }
    public string LogoUrl { get; set; }
}

public class ContactInformation
{
    public bool HasContactForm { get; set; }
    public bool HasMissingAddressContactForm { get; set; }
    public bool HasMissingFloorPlanContactForm { get; set; }
    public int? ContactSiteId { get; set; }
    public string ContactSiteName { get; set; }
    public object ContactSiteLogoUrl { get; set; }
    public int? ContactFormType { get; set; }
    public string DefaultContactMessage { get; set; }
    public object AdvertiserInformation { get; set; }
    public object VendorInformation { get; set; }
    public object VisitationContactInformation { get; set; }
    public bool IsVendorContactForm { get; set; }
    public string OnlineApplicationUrl { get; set; }
    public string OnlineApplicationRemarks { get; set; }
}

public class TargetingInformation
{

}

public class PersistentTargetingInformation
{
    public string key { get; set; }
    public string[] value { get; set; }
    public bool isReplace { get; set; }
}

public class CrossSellingLinks
{
    public string SectionInfoKey { get; set; }
    public Items[] Items { get; set; }
}

public class Items
{
    public string ItemInfoKey { get; set; }
    public string ResultUrl { get; set; }
    public int ServicePartner { get; set; }
}

public class MetaData
{
    public string PageTitle { get; set; }
    public string Description { get; set; }
}

public class Breadcrumbs
{
    public string Title { get; set; }
    public string Href { get; set; }
    public object TargetTrackingCrossLabel { get; set; }
    public bool IsSelected { get; set; }
}

public class Header1
{
    public int StatusCode { get; set; }
    public string DebugMessage { get; set; }
    public object StatusMessage { get; set; }
}

