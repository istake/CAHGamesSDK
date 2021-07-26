var CheckMobile = {
    IsMobile : function()
    {
        return UnityLoader.SystemInfo.mobile;
    }
};
mergeInto(LibraryManager.library, CheckMobile  