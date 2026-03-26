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

# ── Standalone Keys ────────────────────────────────────────────────────────────

resource "etcd_key" "my_key_name" {
  key   = "MyKeyName"
  value = "MyValue"
}

# ── TestSection Keys ───────────────────────────────────────────────────────────

resource "etcd_key" "test_section_item1" {
  key   = "TestSection:Item1"
  value = "Item 1 value"
}

resource "etcd_key" "test_section_item2" {
  key   = "TestSection:Item2"
  value = "Item 2 value"
}

resource "etcd_key" "test_section_subsection_item1" {
  key   = "TestSection:SubSection:Item1"
  value = "Sub section value 1"
}

resource "etcd_key" "test_section_subsection_item2" {
  key   = "TestSection:SubSection:Item2"
  value = "Sub section value 2"
}

resource "etcd_key" "test_section_arraysection_0" {
  key   = "TestSection:ArraySection:0"
  value = "Item 1"
}

resource "etcd_key" "test_section_arraysection_1" {
  key   = "TestSection:ArraySection:1"
  value = "Item 2"
}

# ── MyPrefix Keys ──────────────────────────────────────────────────────────────

resource "etcd_key" "myprefix_testappsection_item1" {
  key   = "MyPrefix:TestAppSection:Item1"
  value = "1234321"
}

# ── MyComplex/Prefix Keys ──────────────────────────────────────────────────────

resource "etcd_key" "mycomplex_prefix_settings_testkey" {
  key   = "MyComplex/Prefix/Settings:TestKey"
  value = "Test value"
}
