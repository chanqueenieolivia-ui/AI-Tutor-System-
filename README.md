name: Unity WebGL Build & Deploy

on:
  push:
    branches:
      - main   # 當 main 分支有更新時觸發

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
      # 1️⃣ 取出程式碼
      - name: Checkout repository
        uses: actions/checkout@v3

      # 2️⃣ 安裝 Unity Builder
      - name: Unity WebGL Build
        uses: game-ci/unity-builder@v2
        with:
          unityVersion: 2021.3.20f1   # 請改成你專案的 Unity 版本
          targetPlatform: WebGL

      # 3️⃣ 儲存建置結果
      - name: Upload build artifact
        uses: actions/upload-artifact@v3
        with:
          name: WebGLBuild
          path: build/WebGL

  deploy:
    runs-on: ubuntu-latest
    needs: build

    steps:
      # 1️⃣ 取出建置結果
      - name: Download build artifact
        uses: actions/download-artifact@v3
        with:
          name: WebGLBuild
          path: build/WebGL

      # 2️⃣ 部署到 GitHub Pages
      - name: Deploy to GitHub Pages
        uses: peaceiris/actions-gh-pages@v3
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_dir: build/WebGL

