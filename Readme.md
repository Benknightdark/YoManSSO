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
# TODO
- 客製化Scope參數
- 回傳客製化錯誤訊息