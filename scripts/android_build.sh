
SCRIPT_FOLDER=$(dirname $(readlink -f $0))
echo $KEYSTORE | base64 --decode > $SCRIPT_FOLDER/keystore.jks
KS=$SCRIPT_FOLDER/keystore.jks

RELEASE_FOLDER=$SCRIPT_FOLDER/../Leaquid.Android/bin/Release
rm -rf $RELEASE_FOLDER

dotnet publish $SCRIPT_FOLDER/../Leaquid.Android/Leaquid.Android.csproj -c Release -p:PublishTrimmed=true \
  -p:AndroidSigningKeyAlias=eklore -p:AndroidSigningKeyPass=env:SIGNINGKEY_PASSWORD \
  -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=$KS -p:AndroidSigningStorePass=env:KEYSTORE_PASSWORD

ls -lR $RELEASE_FOLDER
