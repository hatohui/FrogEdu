variable "project_name" {
  description = "Name of the project, used for resource naming"
  type        = string
}

### CORS Configuration
variable "cors_origins" {
  description = "List of allowed CORS origins for API Gateway"
  type        = list(string)
  default     = ["http://localhost:5173", "http://localhost:5174", "https://frogedu.org", "https://www.frogedu.org"]
}
