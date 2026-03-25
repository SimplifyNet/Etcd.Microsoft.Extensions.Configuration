terraform {
  required_version = "~>1.11.5"
  required_providers {
    etcd = {
      source  = "Ferlab-Ste-Justine/etcd"
      version = "0.11.0"
    }
  }
}

resource "etcd_key" "test_section_item1" {
  key   = "${var.prefix}TestSection${var.delimiter}Item1"
  value = "Item 1 value"
}

resource "etcd_key" "test_section_item2" {
  key   = "${var.prefix}TestSection${var.delimiter}Item2"
  value = "Item 2 value"
}

resource "etcd_key" "test_section_subsection_item1" {
  key   = "${var.prefix}TestSection${var.delimiter}SubSection${var.delimiter}Item1"
  value = "Sub section value 1"
}

resource "etcd_key" "test_section_subsection_item2" {
  key   = "${var.prefix}TestSection${var.delimiter}SubSection${var.delimiter}Item2"
  value = "Sub section value 2"
}

resource "etcd_key" "test_app_settings_item1" {
  key   = "${var.prefix}TestAppSection${var.delimiter}Item1"
  value = "1234321"
}

resource "etcd_key" "array_section_item1" {
  key   = "${var.prefix}ArraySection${var.delimiter}Item1"
  value = "Item 1"
}

resource "etcd_key" "array_section_item2" {
  key   = "${var.prefix}ArraySection${var.delimiter}Item2"
  value = "Item 2"
}

resource "etcd_key" "settings_test_key" {
  key   = "${var.prefix}Settings${var.delimiter}TestKey"
  value = "Test value"
}


