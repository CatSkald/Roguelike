#!/bin/bash

PAKET_PATH=.paket/paket.exe

if test "$OS" = "Windows_NT"
then
  $PAKET_PATH install
  packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
else
  mono $PAKET_PATH install
  mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi
