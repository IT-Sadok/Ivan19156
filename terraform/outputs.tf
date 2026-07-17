output "postgres_host" {
  value = azurerm_postgresql_flexible_server.main.fqdn
}

output "eventhub_bootstrap_server" {
  value = "${azurerm_eventhub_namespace.main.name}.servicebus.windows.net:9093"
}

output "eventhub_connection_string" {
  value     = azurerm_eventhub_namespace.main.default_primary_connection_string
  sensitive = true
}