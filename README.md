# EclipseAPI
Unity3D 使用的流程控制腳本, 能使在製作 Unity 遊戲時更簡單控制遊戲流程.
此插件用於 Unity 3D HDPR


## 功能:
* 中英文除錯輸出
* 控制台介面控制
* 地圖管理
* 介面管理
* 多國語言支援
* 鍵盤按鍵支援
* 插件支援

## 版本更新資訊
### 版本 0.8
* 加入陽光夾角偵測
* 加入地面材質
* 加入測試場景
* 加入預設特效
* 修正角色卡住與位移 Bug
### 版本 0.7
* 加入雙語介面至其他元件
* 加入重力控制
* 加入光源控制
* 加入實用元件
* 加入天空控制
* 修正切換地圖物件無法清乾淨 Bug
* 修正角色基礎
### 版本 0.6
* 加入幫助指令
* 加入指令執行元件
* 加入指令集執行元件
* 加入指令權限
### 版本 0.5
* 加入控制台滑動事件
* 加入角色控制器管理
* 加入指令幫助
* 修正攝影機更新 Bug
### 版本 0.4
* 加入走路聲音
* 加入測試實體物件
* 加入第一人稱角色動畫與控制器
* 加入管理腳本起始呼叫
* 修正地圖標記點流程
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
* 清除控制台紀錄
>>> clear
* 查詢目前權限
>>> permission
* 開啟管理員
>>> admin [0 or 1]
* 查詢指令
>>> help
* 查詢核心指令
>>> help core
* 查詢插件指令
>>> help plugin
* 查詢世界指令
>>> help world
* 查詢音源指令
>>> help audio
* 查詢實體指令
>>> help entity
* 查詢玩家指令
>>> help player
* 查詢角色指令
>>> help character
### 插件
* 查看插件選單
>>> plugin list
### 世界
* 查看世界預設角色
>>> world default [角色ID]
* 查看世界清單
>>> world list
* 生成世界 
>>> world spawn [世界ID] 
* 世界重生點
>>> world respawn [x] [y] [z]
* 刪除世界
>>> world clear
* 天空列表
>>> world sky list
* 設定天空
>>> world sky set [天空ID]
* 設定三維重力
>>> gravity [x] [y] [z]
* 設定一維重力
>>> gravity [y]
### 音源
* 音樂列表
>>> audio music list
* 音效列表
>>> audio sfx list
* 調整音樂音量 音量介於 0 - 1 之間
>>> audio music [音量] 
* 播放音樂
>>> audio music play [音樂ID] 
* 停止所有音樂
>>> audio music stop
* 調整音效音量 音量介於 0 - 1 之間
>>> audio sfx [音量] 
* 生成音效 但是是全場都聽得見的 2D 音效
>>> audio sfx play [音效ID] 
* 生成音效到指定位置 3D 音效
>>> audio sfx play [音效ID] [x] [y] [z] 
### 實體
* 生成實體物件到玩家面前
>>> entity spawn [實體物件] 
* 生成實體物件到指定位置
>>> entity spawn [實體物件] [x] [y] [z] 
* 刪除所有實體物件
>>> entity clear
### 玩家
* 飛行幽靈模式
>>> noclip
* 生成玩家
>>> spawn player [x] [y] [z] 
* 傳送玩家
>>> teleport [x] [y] [z] 
