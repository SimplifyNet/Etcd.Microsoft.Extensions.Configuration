terraform {
  required_version = "~> 1.11"
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
  source = "./settings"
}

module "prefix1" {
  source = "./settings"
  prefix = "MyPrefix:"
}

module "prefix2" {
  source = "./settings"
  prefix = "MYCOMPLEX/prefix/"
}

