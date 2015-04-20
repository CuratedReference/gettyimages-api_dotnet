namespace GettyImages.Connect.Search
{
    public interface IBlendedImagesSearch : IImagesSearch
    {
        new IBlendedImagesSearch WithPage(int val);
        new IBlendedImagesSearch WithPageSize(int val);
        new IBlendedImagesSearch WithPhrase(string val);
        new IBlendedImagesSearch WithSortOrder(string val);
        new IBlendedImagesSearch WithEmbedContentOnly(bool value = true);
        new IBlendedImagesSearch WithExcludeNudity(bool value = true);
        new IBlendedImagesSearch WithResponseField(string field);
        new IBlendedImagesSearch WithGraphicalStyle(GraphicalStyles value);
        new IBlendedImagesSearch WithOrientation(Orientation value);
        new IBlendedImagesSearch WithEventId(int value);
        new IBlendedImagesSearch WithFileType(FileType value);
        new IBlendedImagesSearch WithKeywordId(int value);
        new IBlendedImagesSearch WithNumberOfPeople(NumberOfPeople value);
        new IBlendedImagesSearch WithPrestigeContentOnly(bool value = true);
        new IBlendedImagesSearch WithProductType(ProductType value);
        new IBlendedImagesSearch WithLocation(string value);
        IBlendedImagesSearch WithLicenseModel(LicenseModel value);
        ICreativeImagesSearch Creative();
        IEditorialImagesSearch Editorial();
    }
}