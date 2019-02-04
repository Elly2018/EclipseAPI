# EclipseAPI
Unity3D 使用的流程控制腳本, 能使在製作 Unity 遊戲時更簡單控制遊戲流程.

## 功能:
* 中英文除錯輸出
* 控制台介面控制
* 地圖管理
* 介面管理
* 多國語言支援
* 鍵盤按鍵支援
* 插件支援

## 版本更新資訊
### 版本 0.4
* 加入第一人稱角色動畫與控制器
* 加入管理腳本起始呼叫
* 修正實體物件生成 Bug
* 修正音源生成 Bug
### 版本 0.3
* 加入實體管理腳本
* 加入角色離開世界死亡設定 ( y 過低時)
* 加入載入插件指令功能
* 加入載入插件按鍵功能
* 修正指令瀏覽 GUI Bug
### 版本 0.2
* 修正GUI語言
* 修正控制台
* 加入內建地圖 DemoMap
* 加入第一人稱角色 (基礎)
### 版本 0.1
* 加入中文GUI介面 (介面目前只支援中文)
* 加入多國語言標籤結構 (尚未支援載入語言包)
* 加入音源管理 (分為音樂與音效)
* 加入介面管理腳本
* 加入地圖生成管理
* 加入插件管理腳本 (可使用 Eclipse 的插件)
* 加入控制台 

## 控制台選單
### 控制台
1. 清除控制台紀錄
>>> clear
### 世界
1. 生成世界 
>>> world spawn [世界ID] 
### 音源
1. 調整音樂音量 音量介於 0 - 1 之間
* audio music [音量] 
2. 播放音樂
* audio music play [音樂ID] 
3. 停止所有音樂
* audio music stop
4. 調整音效音量 音量介於 0 - 1 之間
* audio sfx [音量] 
5. 生成音效 但是是全場都聽得見的 2D 音效
* audio sfx play [音效ID] 
6. 生成音效到指定位置 3D 音效
* audio sfx play [音效ID] [x] [y] [z] 
### 實體
1. 生成實體物件到玩家面前
* entity spawn [實體物件] 
2. 生成實體物件到指定位置
* entity spawn [實體物件] [x] [y] [z] 
### 玩家
1. 生成玩家
* spawn player [x] [y] [z] 
2. 傳送玩家
* teleport [x] [y] [z] 
