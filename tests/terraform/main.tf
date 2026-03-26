terraform {
  required_version = "~>1.11.5"
  required_providers {
    etcd = {
      source  = "Ferlab-Ste-Justine/etcd"
      version = "0.11.0"
    }
  }
}


provider "etcd" {
  username  = "root"
  password  = "snBr8ss9ls"
  endpoints = "http://localhost:2379"
  skip_tls  = true
}

module "settings" {
  source    = "./settings"
  prefix    = ""
  delimiter = ":"
}

module "prefix1" {
  source    = "./settings"
  prefix    = "MyPrefix:"
  delimiter = ":"
}

module "prefix2" {
  source    = "./settings"
  prefix    = "MYCOMPLEX/prefix/"
  delimiter = ":"
}

