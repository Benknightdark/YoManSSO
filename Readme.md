# 已支援的SSO
- ✔️ Line
- ✔️ FB
- ✔️ Google
- ✔️ LinkedIn
- ✔️ MS 
# 範例程式
 - https://github.com/Benknightdark/YoManSSOExample
# Nuget下載網址
- https://www.nuget.org/packages/YoManSSO/
# 使用方式
* 加入服務
  ```
  // 在Startup.cs檔的ConfigureServices Method加入以下程式

  // 讀取OAuth2 appsetting.json參數資料的Service
  SSOServiceCollections.AddSSOConfig (services);

  // Line相關的Service
  SSOServiceCollections.AddLine (services);

  // Facebook相關的Service
  SSOServiceCollections.AddFB (services);

  // Google相關的Service
  SSOServiceCollections.AddGoogle (services);

  // LinkedIn相關的Service
  SSOServiceCollections.AddLinkedIn (services);

  // Microsoft相關的Service
  SSOServiceCollections.AddMS (services);
  ```
* 在所有的appsetting.json檔加入OAuth2應用程式的參數 (微軟需要再加上租用戶識別碼)
```json
"Line": {
    "ClientID": "[Your Line App ClientID]",
    "ClientSecret": "[Your Line App ClientSecret]",
    "RedirectUrl": "[Your Line App Redirect Url]"
  },
  "FB": {
   "ClientID": "[Your FB App ClientID]",
    "ClientSecret": "[Your FB App ClientSecret]",
    "RedirectUrl": "[Your FB App Redirect Url]"
  },
  "Google": {
    "ClientID": "[Your Google App ClientID]",
    "ClientSecret": "[Your Google App ClientSecret]",
    "RedirectUrl": "[Your Google App Redirect Url]"
  },
  "LinkedIn": {
     "ClientID": "[Your LinkedIn App ClientID]",
    "ClientSecret": "[Your LinkedIn App ClientSecret]",
    "RedirectUrl": "[Your LinkedIn App Redirect Url]"
  },
  "MS": {
    "ClientID": "[Your Micorsoft App ClientID]",
    "TenantID": "[Your Micorsoft TenantID]",
    "ClientSecret": "[Your Micorsoft App ClientSecret]",
    "RedirectUrl": "[Your Micorsoft App Redirect Url]"
  }
```
* 注入服務
``` C#
        // Line
        private LineService _lineService ;
        // Facebook
        private FBService _fBService ;
        // Google
        private GoogleService _googleService ;
        // LinkedIn
        private LinkedInService _linkedService ;
        // Microsoft
        private MSService _msService;
        public HomeController ( LineService lineService, FBService fBService, GoogleService googleService, LinkedInService linkedService, MSService msService) {
            _lineService = lineService;
            _fBService = fBService;
            _googleService = googleService;
            _linkedService = linkedService;
            _msService = msService;
        }
```
* 取得OAuth2 登入畫面網址
``` c#
// Line OAuth2 登入畫面網址
string LineAccessUrl = await _lineService.GetAccessUrl ();
// Facebook OAuth2 登入畫面網址
string FBAccessUrl = await _fBService.GetAccessUrl ();
// Google OAuth2 登入畫面網址
string GoogelAccessUrl = await _googleService.GetAccessUrl ();
// LinkedIn OAuth2 登入畫面網址
string LinkedAccessUrl = await _linkedService.GetAccessUrl ();
// Microsoft OAuth2 登入畫面網址
string MSAccessUrl = await _msService.GetAccessUrl ();
```

* 取得AccessToken和使用者資料
``` c#
// Line 
// 授權碼
string LineAuthorizeCode="12312312vvvv";
// AccessToken
var LineAccessToken = await _lineService.GetAccessToken (LineAuthorizeCode);
// Line使用者資料
var LineUserProfile=await _lineService.GetProfile (LineAccessToken);
// Line使用者Email
var LineUserEmail=await _lineService.GetEmail(LineAccessToken);

// LinkedIn
// 授權碼
string LinkedInAuthorizeCode="@#$%";
// AccessToken
var LinkedInAccessToken = await _linkedInService.GetAccessToken (LinkedInAuthorizeCode);
// LinkedIn使用者資料
var LinkedInUserProfile=await _linkedInService.GetProfile (LinkedInAccessToken);

// Microsoft
// 授權碼
string MSAuthorizeCode="12312312vvvv";
var MSAccessToken = await _msService.GetAccessToken (MSAuthorizeCode);
// Microsoft使用者資料
var MSleUserProfile=await _msService.GetProfile (MSAccessToken);

// Google
// 授權碼
string GoogleAuthorizeCode="POIJH00";
// AccessToken
var GoogleAccessToken = await _googleService.GetAccessToken (GoogleAuthorizeCode);
// Google使用者資料
var GoogleUserProfile=await _googleService.GetProfile (GoogleAccessToken);

// Facebook
// 授權碼
string FBAuthorizeCode="%^&*";
// AccessToken
var FBAccessToken = await _fBService.GetAccessToken (FBAuthorizeCode);
// Facebook使用者資料
var FBUserProfile=await _fBService.GetProfile (FBAccessToken);
```
# TODO
- 客製化Scope參數
- 回傳客製化錯誤訊息