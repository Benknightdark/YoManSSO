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
* 注入服務
  ```
  // 在Startup.cs檔的ConfigureServices Method加入以下程式

  // 讀取OAuth2 appsetting.json的Service
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
# TODO
- 客製化Scope參數
- 回傳客製化錯誤訊息