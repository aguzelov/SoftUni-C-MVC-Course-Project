﻿using System.Collections.Generic;

namespace Dialog.Common
{
    public static class GlobalConstants
    {
        public const string AdminRole = "Admin";
        public static readonly string UserRole = "User";

        //Settings
        public static readonly string ApplicationNameKey = "ApplicationName";

        public static readonly string ApplicationSloganKey = "ApplicationSlogan";

        public static readonly string ApplicationAddressKey = "ApplicationAddress";
        public static readonly string ApplicationPhoneKey = "ApplicationPhone";
        public static readonly string ApplicationEmailKey = "ApplicationEmail";
        public static readonly string ApplicationAboutFooterKey = "ApplicationAboutFooter";

        public static readonly string IndexPostsCountKey = "IndexPostsCount";
        public static readonly string IndexNewsCountKey = "IndexNewsCount";
        public static readonly string IndexImagesCountKey = "IndexImagesCount";
        public static readonly string RecentPostsCountKey = "RecentPostsCount";

        public static readonly string DefaultPostImageKey = "DefaultPostImage";
        public static readonly string DefaultNewsImageKey = "DefaultNewsImage";

        public static readonly string AllEntitiesPageSizeKey = "AllEntitiesPageSize";

        //CacheKeys

        public static readonly string ContactInfo = "ContactInfo";
        public static readonly int ContactInfoCacheExpirationDay = 1;
        public static readonly string RecentBlogPost = "RecentBlogPost";
        public static readonly int RecentBlogCacheExpirationDay = 1;
        public static readonly string ApplicationInfo = "ApplicationInfo";
        public static readonly int ApplicationInfoCacheExpirationDay = 10;
        public static readonly string IndexRecentEntities = "IndexRecentEntities";
        public static readonly int IndexRecentEntitiesCacheExpirationDay = 1;

        //Chat

        public static readonly string GlobalChatRoomName = "Global";
        public static readonly int RecentMessageCount = 10;

        //SendGrid
        public static readonly string SendEmailFromName = "SendEmailFromName";

        public static readonly string SendEmailFromAdress = "SendEmailFromAdress";
        //

        public static readonly string ModelStateServiceResult = "Result";
        public static readonly string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";

        //Areas
        public static readonly string AdministrationArea = "Administration";

        public static readonly string BlogArea = "Blog";
        public static readonly string NewsArea = "News";
        public static readonly string GalleryArea = "Gallery";

        //Controllers
        public static readonly string BlogController = "Blog";

        public static readonly string NewsController = "News";
        public static readonly string GalleryController = "Gallery";
        public static readonly string AdministratorController = "Administrator";

        //Errors
        public static readonly string ModelIsEmpty = "Model is empty!";

        public static readonly string EntityIsNotFound = "{0} is not found!";
        public static readonly string EntityNotSaved = "{0} not saved in database!";
        public static readonly string InvalidParameter = "Invalid parameter: {0}";

        //
        public static readonly string JoinSeparator = "; ";
    }
}