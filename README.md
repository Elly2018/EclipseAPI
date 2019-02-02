# EclipseAPI
Unity3D 使用的流程控制腳本, 能使在製作 Unity 遊戲時更簡單控制遊戲流程.

## 功能:
* 中文除錯輸出
* 控制台介面控制
* 地圖管理
* 介面管理
* 多國語言管理
* 鍵盤按鍵管理

## 版本更新資訊
### 版本 0.1
* 加入中文GUI介面 (介面目前只支援中文)
* 加入多國語言標籤結構 (尚未支援載入語言包)
* 加入音源管理 (分為音樂與音效)
* 加入介面管理腳本
* 加入地圖生成管理
* 加入插件管理腳本 (可使用 Eclipse 的插件)
* 加入控制台 
### 版本 0.2
* 更新GUI語言
* 更新控制台
* 加入內建地圖 DemoMap
* 加入第一人稱角色 (基礎)
### 版本 0.3
* 加入實體管理腳本
* 加入角色離開世界死亡設定 ( y 低於 200)
* 加入載入插件指令功能
* 加入載入插件按鍵功能
* 修正指令瀏覽 GUI Bug

## 控制台選單
* clear 清除控制台紀錄
* world spawn [世界ID] 生成世界
* audio music [音量] 調整音樂音量 音量介於 0 - 1 之間
* audio sfx [音量] 調整音效音量 音量介於 0 - 1 之間
* spawn player [x] [y] [z] 生成玩家
* teleport [x] [y] [z] 傳送玩家
