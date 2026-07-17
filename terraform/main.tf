terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = "cd19b9c3-57da-4c79-873f-5fb8b17005c4"
}

resource "azurerm_resource_group" "main" {
  name     = "iot-rg"
  location = "swedencentral"
}


resource "azurerm_container_registry" "main" {
  name                = "iotregistryi"
  resource_group_name = azurerm_resource_group.main.name
  location            = "Sweden Central"
  sku                 = "Basic"
  admin_enabled       = true
}

resource "azurerm_kubernetes_cluster" "main" {
  name                      = "iot-aks"
  location                  = "northeurope"
  resource_group_name       = azurerm_resource_group.main.name
  dns_prefix                = "iot-aks-dns"
  oidc_issuer_enabled       = true
  workload_identity_enabled = true

  default_node_pool {
    name                        = "agentpool"
    node_count                  = 1
    vm_size                     = "Standard_D2s_v3"
    temporary_name_for_rotation = "temppool"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_postgresql_flexible_server" "main" {
  name                   = "iot-postgres"
  resource_group_name    = azurerm_resource_group.main.name
  location               = "northeurope"
  version                = "16"
  administrator_login    = "iot_user"
  administrator_password = var.postgres_password
  sku_name               = "B_Standard_B2s"
  storage_mb             = 32768
  zone                   = "2"

  authentication {
    active_directory_auth_enabled = false
    password_auth_enabled         = true
  }
}

resource "azurerm_postgresql_flexible_server_configuration" "vector" {
  name      = "azure.extensions"
  server_id = azurerm_postgresql_flexible_server.main.id
  value     = "VECTOR"
}

resource "azurerm_postgresql_flexible_server_database" "main" {
  name      = "iot_db"
  server_id = azurerm_postgresql_flexible_server.main.id
  charset   = "UTF8"
  collation = "en_US.utf8"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "allow_azure" {
  name             = "allow-azure-services"
  server_id        = azurerm_postgresql_flexible_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}


resource "azurerm_eventhub_namespace" "main" {
  name                = "iot-eventhubs"
  location = "northeurope"
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "Standard"
  capacity            = 1
}

resource "azurerm_eventhub" "telemetry" {
  name                = "iot.telemetry"
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
  partition_count     = 1
  message_retention   = 1
}

resource "azurerm_eventhub" "embedding" {
  name                = "iot.embedding-generation"
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
  partition_count     = 1
  message_retention   = 1
}