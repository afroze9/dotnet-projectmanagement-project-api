$token = "13585e24-52a3-e906-981a-b2246ed54d77"

consul acl policy create -name="kv-project-api" -description="Policy that grants KV access to project-api" -rules @project-api-policies.hcl -token="$token"
consul acl token create -policy-name="kv-project-api" -service-identity="project-api" -token="$token"

consul kv put -token="$token" project-api/app-config @app-config.json
