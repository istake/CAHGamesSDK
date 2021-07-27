# [CAHGamesSDK](Assets/CAH)
 CAHGames Unity Common SDK
 
 유니티 게임 개발을 위한 SDK, 다중 플랫폼 결제, 여러 플랫폼 API 서비스, 커먼 게임시스템, WebGL 관련한 플러그인, 파이어베이스 플러그인 적용
 
 본 SDK는 본인 프로젝트에 필요한 폴더만 Export하여 적용할 수 있는 최대한 종속성을 피해 만든 라이브러리입니다.  
 
 단, 각 모듈에서 필요로하는 (IAP의 경우 유니티 IAP) 라이브러리를 임포트 해서 사용해야 합니다.
 
 
 앞으로 게임을 만들게 되면서 필요한 기능을 구현하고 모듈화 시킬때 작업할 예정이라 모든 구현사항이 들어가있지는 않지만
 
 기존 평소 필요하다고 느낀것들은 지속적으로 추가할 예정입니다.
 

 ⚠️ = Need Test  
 ✔️ = Tested, Working Fine
 
 # Details
  
 ## [IAP](/Assets/CAH/IAP)
  - Google Support ✔️  
  - Appstore Support ⚠️
 
 
 ## [Firebase](/Assets/CAH/Firebase)
  - Auth ✔️ 
  - Analytics
  - EventLog  
  
 ## [Auth](/Assets/CAH/Auth)
 - Google✔️ 
 - Appstore 



 ### Memo
 
  - Plugins들은 최대한 Assets/ 에 위치해야 Resolver 관련 문제가 생기지 않음 
