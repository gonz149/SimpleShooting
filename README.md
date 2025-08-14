# SimpleShooting - 3Dインベーダーゲーム

3D空間で展開されるインベーダーゲームプロジェクトです。プレイヤーは宇宙船を操縦し、迫り来る敵の集団を撃破する古典的なシューティングゲームを3D環境で再現しています。

## プロジェクト概要

このプロジェクトは、Unity 6000.0.51f1とURP（Universal Render Pipeline）を使用して開発されています。Input Systemを利用した現代的な入力処理と、3D物理演算による弾道システムを実装しています。

## ゲーム仕様

### 基本コンセプト
- **3D空間**: 奥行きのあるゲームステージでの戦闘
- **プレイヤー移動**: X軸（左右）方向の移動
- **敵集団行動**: X軸・Z軸を使った集団移動パターン
- **物理ベース**: Rigidbody・Colliderを使用した3D物理演算

### 操作方法
- **移動**: WASD / 矢印キー / ゲームパッド左スティック
- **射撃**: マウス左クリック / Enter / ゲームパッドXボタン / タッチ

## 技術仕様

### 開発環境
- **Unity**: 6000.0.51f1
- **レンダーパイプライン**: Universal Render Pipeline (URP)
- **入力システム**: Unity Input System
- **プログラミング言語**: C#

### アーキテクチャ

プロジェクトはSOLID原則、YAGNI原則、DRY原則に基づいて設計されており、責任の分離と拡張性を重視しています。

#### インターフェース設計（Interface Segregation Principle）

各機能は特化したインターフェースで分離されています：

- **`IDamageable`** - ダメージ処理専用
  - `void TakeDamage(float damage)`
  - `bool IsDestroyed`

- **`IWeaponSystem`** - 武器システム専用
  - `bool CanFire`
  - `void Fire()`
  - `void SetTarget(Transform target)`

- **`IMovement`** - 移動機能専用
  - `void Move(Vector3 direction)`
  - `void SetSpeed(float speed)`
  - `Vector3 CurrentVelocity`

- **`ITargetProvider`** - ターゲット提供専用
  - `Transform GetTarget()`
  - `bool HasTarget`

#### コアクラス設計（Single Responsibility Principle）

**プレイヤーシステム**
- **`PlayerController`** - 入力処理と各システムの調整（IDamageableを実装）
- **`PlayerMovement`** - プレイヤー移動制御のみ（IMovementを実装）

**敵システム**
- **`Enemy`** - 敵の行動制御（IDamageableを実装）
- **`PlayerTargetProvider`** - プレイヤー検索ロジック（ITargetProviderを実装）

**武器システム（DRY原則による共通化）**
- **`WeaponSystem`** - 武器発射システム（プレイヤー・敵で共通利用、IWeaponSystemを実装）

**弾システム（Open/Closed Principle）**
- **`BaseBullet`** - 弾の基底クラス（拡張に開放、修正に閉鎖）
- **`PlayerBullet`** - プレイヤー弾（BaseBulletを継承）
- **`EnemyBullet`** - 敵弾（BaseBulletを継承）

**管理システム**
- **`EnemySpawner`** - 敵集団のグリッド配置
- **`EnemySwarm`** - 敵集団の移動ロジック

#### 依存性設計（Dependency Inversion Principle）

高水準モジュールは抽象（インターフェース）に依存し、具象クラスには依存しません：

```csharp
// PlayerControllerの例
IMovement movement;           // 具象のPlayerMovementではなく抽象に依存
IWeaponSystem weapon;         // 具象のWeaponSystemではなく抽象に依存

// Enemyの例  
IWeaponSystem weapon;         // 武器システムを共通利用
ITargetProvider target;       // ターゲット検索の抽象化
```

#### 責任分離の実装例

**弾のヒット処理（修正前の問題）**
```csharp
// 問題: 弾が対象の破壊を直接実行（責任違反）
Destroy(hitObject);  // ❌ 弾の責務を超えている
```

**弾のヒット処理（修正後）**
```csharp
// 解決: 弾は通知のみ、破壊判定は受ける側の責務
IDamageable damageable = hitObject.GetComponent<IDamageable>();
damageable.TakeDamage(damage);  // ✅ 適切な責任分離
```

#### プレハブ構成
- `Player.prefab` - PlayerController + PlayerMovement + WeaponSystem
- `PlayerBullet.prefab` - PlayerBullet (BaseBulletを継承)
- `Enemy.prefab` - Enemy + WeaponSystem + PlayerTargetProvider  
- `EnemyBullet.prefab` - EnemyBullet (BaseBulletを継承)

### フォルダ構造
```
Assets/Gohan/               # プロジェクト専用アセット
├── Scripts/               # C#スクリプト
├── Prefabs/              # ゲームオブジェクトプレハブ
├── Materials/            # マテリアル
└── Models/              # 3Dモデル
```

## 実装状況

### 完成済み機能
- ✅ プレイヤー移動システム（X軸制限）
- ✅ Input Systemによる操作
- ✅ 弾発射・物理演算
- ✅ 敵のグリッド配置
- ✅ 敵集団移動ロジック
- ✅ 弾と敵の衝突判定
- ✅ 敵からの攻撃システム

### 計画中の機能
- GameManagerによるゲーム状態管理
- スコア・ライフシステム
- UIの実装
- 防壁（シールド）システム
- ゲームクリア・ゲームオーバー判定

## 設計思想

### SOLID原則による堅牢な設計

#### Single Responsibility Principle（単一責任原則）
各クラスは1つの責任のみを持ちます：
- `PlayerController` - 入力処理と各システムの調整
- `PlayerMovement` - プレイヤー移動制御のみ
- `WeaponSystem` - 武器発射システムのみ
- `BaseBullet` - 弾の基本動作のみ

#### Open/Closed Principle（開放/閉鎖原則）
拡張に対して開放的、修正に対して閉鎖的：
- `BaseBullet`を継承してPlayerBullet、EnemyBulletを実装
- 既存コードを変更せずに新機能を追加可能

#### Liskov Substitution Principle（リスコフの置換原則）
派生クラスは基底クラスと置換可能：
- `IDamageable`を実装したクラスは期待される動作を維持
- インターフェース契約を破らない実装

#### Interface Segregation Principle（インターフェース分離原則）
クライアントは利用しないメソッドに依存しない：
- `IDamageable` - ダメージ処理のみ
- `IWeaponSystem` - 武器機能のみ
- `IMovement` - 移動機能のみ
- `ITargetProvider` - ターゲット提供のみ

#### Dependency Inversion Principle（依存性逆転原則）
具象ではなく抽象に依存：
- コンポーネント間はインターフェースを通じて連携
- 依存性注入によるテスタビリティ向上

### YAGNI・DRY原則による効率的な開発

#### YAGNI（You Aren't Gonna Need It）
現在必要でない機能は実装しない：
- 推測による機能追加を避ける
- 実際の要求に基づいて開発
- シンプルで直接的な実装を選択

#### DRY（Don't Repeat Yourself）
コードの重複を避け、情報の一意性を保つ：
- `WeaponSystem`クラスをプレイヤーと敵で共通利用
- `IDamageable`インターフェースでダメージ処理を共通化
- 設定値はSerializeFieldで一元管理

### 3D空間の活用
従来の2Dインベーダーゲームを3D空間に拡張：
- 奥行きを活かした敵の前進システム
- 3D物理演算による自然な弾道
- カメラワークの可能性を拡張

### 拡張性と保守性
将来的な機能追加を見据えた構造：
- インターフェースベースの疎結合設計
- 責任分離による変更容易性
- SOLID原則による安全な機能拡張

## 開発ルール

### アセット管理
- すべてのカスタムアセットは `Assets/Gohan` フォルダ内に配置
- 外部アセットとの明確な分離
- プレハブベースのオブジェクト管理

### コーディング規約

#### SOLID原則準拠
- **S** - 単一責任原則：1つのクラスは1つの責任のみ
- **O** - 開放/閉鎖原則：継承やインターフェースで機能拡張
- **L** - リスコフの置換原則：派生クラスは基底クラスと置換可能
- **I** - インターフェース分離原則：利用しないメソッドへの依存を回避
- **D** - 依存性逆転原則：抽象（インターフェース）に依存

#### YAGNI・DRY原則準拠
- **YAGNI**：現在必要でない機能は実装しない
- **DRY**：同じロジックや情報を複数箇所に書かない

#### 実装ガイドライン
- **アクセスレベル**：privateは明示しない（デフォルトのため）
- **依存性注入**：コンポーネント参照はインターフェースを使用
- **Input System**：現代的な入力処理システムを活用
- **Inspector設定**：SerializeFieldで設定値を露出
- **タグベース判定**：衝突判定はCompareTagを使用

## プロジェクト履歴

詳細な開発計画については `plan.md` を参照してください。UnityMCPを使用した開発環境の統合により、効率的な開発フローを実現しています。