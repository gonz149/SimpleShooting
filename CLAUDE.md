# プロジェクトルール・慣例

## アセット管理

### フォルダ構成
- **作成するすべてのアセットは `Assets/Gohan` フォルダ内に配置する**
  - スクリプト、マテリアル、プレハブ、シェーダーなど全てのカスタムアセット
  - 外部アセット（Asset Store、パッケージなど）と明確に区別するため

## 開発ガイドライン

### 命名規則
- [今後追加予定]

### コーディング規約

#### SOLID原則に準拠する
すべてのコードはSOLID原則に従って設計・実装する：

**S - Single Responsibility Principle（単一責任原則）**
- 1つのクラスは1つの責任のみを持つ
- 例：`PlayerController`は移動制御のみ、`WeaponSystem`は武器システムのみ
- 変更理由が複数ある場合はクラスを分割する

**O - Open/Closed Principle（開放/閉鎖原則）**
- クラスは拡張に対して開放的、修正に対して閉鎖的であるべき
- 継承やインターフェースを活用して機能拡張する
- 既存コードを変更せずに新機能を追加できる設計にする

**L - Liskov Substitution Principle（リスコフの置換原則）**
- 派生クラスは基底クラスと置換可能でなければならない
- 継承関係において、子クラスは親クラスの契約を破ってはならない
- インターフェースの実装では期待される動作を維持する

**I - Interface Segregation Principle（インターフェース分離原則）**
- クライアントは利用しないメソッドへの依存を強制されるべきではない
- 大きなインターフェースを小さな特化したインターフェースに分割する
- 必要な機能のみを公開する

**D - Dependency Inversion Principle（依存性逆転原則）**
- 高水準モジュールは低水準モジュールに依存すべきではない
- 両方とも抽象に依存すべきである
- 具象クラスではなくインターフェースや抽象クラスに依存する

#### 実装ガイドライン
- MonoBehaviourの責任を明確に分離する
- ScriptableObjectを活用してデータと振る舞いを分離する
- イベント駆動設計でコンポーネント間の疎結合を実現する
- UnityのComponent Systemを活用した構成パターンを採用する

#### 現在のアーキテクチャ

**インターフェース設計**:
- `IDamageable`: ダメージ処理専用（Interface Segregation）
- `IWeaponSystem`: 武器システム専用
- `IMovement`: 移動機能専用
- `ITargetProvider`: ターゲット提供専用

**コンポーネント構成**:
- `PlayerController`: 入力処理と各システム調整（Single Responsibility）
- `PlayerMovement`: プレイヤー移動制御のみ
- `WeaponSystem`: 武器発射システム（再利用可能）
- `Enemy`: 敵の行動制御（IDamageableを実装）
- `PlayerTargetProvider`: プレイヤー検索ロジック

**弾システム**:
- `BaseBullet`: 弾の基底クラス（Open/Closed Principle）
- `PlayerBullet`: プレイヤー弾（BaseBulletを継承）
- `EnemyBullet`: 敵弾（BaseBulletを継承）

**依存性管理**:
- インターフェースへの依存（Dependency Inversion）
- コンポーネント参照での依存性注入
- 具象クラスに直接依存しない設計

**推奨プレハブ構成**:
```
Player GameObject:
├── PlayerMovement
├── WeaponSystem
├── PlayerController
└── (IDamageableの実装)

Enemy GameObject:
├── WeaponSystem
├── PlayerTargetProvider
└── Enemy

Bullet GameObject:
└── PlayerBullet or EnemyBullet
```

## テスト・ビルド

### 実行コマンド
- [今後追加予定]