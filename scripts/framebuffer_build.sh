
SCRIPT_FOLDER=$(dirname $(readlink -f $0))

RELEASE_FOLDER=$SCRIPT_FOLDER/../Leaquid.Framebuffer/bin/Release
rm -rf $RELEASE_FOLDER

dotnet publish $SCRIPT_FOLDER/../Leaquid.Framebuffer/Leaquid.Framebuffer.csproj -c Release \
  -r linux-arm64 --self-contained true -p:PublishSingleFile=true

ls -lh $RELEASE_FOLDER/net10.0/linux-arm64/publish
