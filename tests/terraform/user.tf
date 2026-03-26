

data "etcd_prefix_range_end" "testsection" {
  key = "TestSection"
}

data "etcd_prefix_range_end" "testappsettings" {
  key = "TestAppSection"
}

data "etcd_prefix_range_end" "arraysection" {
  key = "ArraySection"
}

data "etcd_prefix_range_end" "settings" {
  key = "Settings"
}

data "etcd_prefix_range_end" "myprefix" {
  key = "MyPrefix"
}

data "etcd_prefix_range_end" "complexmyprefix" {
  key = "MYCOMPLEX"
}


resource "etcd_role" "tester" {
  name = "Tester"

  permissions {
    permission = "read"
    key        = data.etcd_prefix_range_end.testsection.key
    range_end  = data.etcd_prefix_range_end.testsection.range_end
  }

  permissions {
    permission = "read"
    key        = data.etcd_prefix_range_end.testappsettings.key
    range_end  = data.etcd_prefix_range_end.testappsettings.range_end
  }

  permissions {
    permission = "read"
    key        = data.etcd_prefix_range_end.arraysection.key
    range_end  = data.etcd_prefix_range_end.arraysection.range_end
  }

  permissions {
    permission = "read"
    key        = data.etcd_prefix_range_end.settings.key
    range_end  = data.etcd_prefix_range_end.settings.range_end
  }

  permissions {
    permission = "read"
    key        = data.etcd_prefix_range_end.myprefix.key
    range_end  = data.etcd_prefix_range_end.myprefix.range_end
  }

  permissions {
    permission = "read"
    key        = data.etcd_prefix_range_end.complexmyprefix.key
    range_end  = data.etcd_prefix_range_end.complexmyprefix.range_end
  }
}

resource "etcd_user" "user" {
  username = "MyUserName"
  password = "passw"
  roles    = [etcd_role.tester.name]
}

