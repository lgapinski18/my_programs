#version 430 core
out vec4 color;

//in vec4 inColor;
in vec2 TexCoords;
in vec3 Normal;
in vec3 Position;

uniform sampler2D texture_diffuse1;

//uniform float Kd;
//uniform float Ks;
uniform vec4 AmbientColor; 
uniform float Ka;
uniform float n;

uniform vec3 PlayerPosition;

uniform bool PointLightOn;
uniform vec4 PointLightColor; 
uniform vec3 PointLightPosition; 
uniform float PointLightRadius; 
uniform float PointLightPower;

uniform bool SpotLightOn1;
uniform vec4 SpotLightColor1;
uniform vec3 SpotLightPos1;
uniform vec3 SpotLightDir1;
uniform float CosConeAngle1;
uniform float concentration1;
uniform float SpotLightRadius1; 
uniform float SpotLightPower1;

uniform bool SpotLightOn2;
uniform vec4 SpotLightColor2;
uniform vec3 SpotLightPos2;
uniform vec3 SpotLightDir2;
uniform float CosConeAngle2;
uniform float concentration2;
uniform float SpotLightRadius2; 
uniform float SpotLightPower2;

uniform bool DirectionalLightOn;
uniform vec4 DirectionalLightColor; 
uniform vec3 DirectionalLightDir; 
uniform float DirectionalLightPower;

vec3 diffuseColor = vec3(0.5, 0.7, 0.2);
vec3 specColor = vec3(1.0, 1.0, 1.0);

vec4 OR(vec4 v1, vec4 v2) {
    return vec4(max(v1.x, v2.x), max(v1.y, v2.y), max(v1.z, v2.z), 1.0f);
} 

vec3 OR(vec3 v1, vec3 v2) {
    return vec3(max(v1.x, v2.x), max(v1.y, v2.y), max(v1.z, v2.z));
}

vec4 AND(vec4 v1, vec4 v2) {
    return vec4(min(v1.x, v2.x), min(v1.y, v2.y), min(v1.z, v2.z), 1.0f);
} 

vec3 AND(vec3 v1, vec3 v2) {
    return vec3(min(v1.x, v2.x), min(v1.y, v2.y), min(v1.z, v2.z));
}

float countLambertianPart(vec3 L, vec3 N) {
    return max(dot(N, L), 0.0f);
}

float countBlinnPhongPart(vec3 L, vec3 E, vec3 N) {
    vec3 H = normalize(L + E);
    float specAngle = max(dot(H, N), 0.0);
    return pow(specAngle, n);
}

void main()
{    
    vec4 fLightColor = vec4(0, 0, 0, 1);

    vec3 N = normalize(Normal);

    vec3 E = normalize(PlayerPosition - Position);

    
    if (PointLightOn) {
        vec3 L = PointLightPosition - Position;
        float distance = length(L);
        L = normalize(L);

        float distanceFactor = max(PointLightRadius - distance, 0.0f) / PointLightRadius;
        float brithnessFactor = distanceFactor * distanceFactor;

        float lambertian = countLambertianPart(L, N);
        float specular = 0.0;

        if (lambertian > 0.0) {
            specular = countBlinnPhongPart(L, E, N);
        }

        fLightColor += vec4((lambertian + specular) * vec3(PointLightColor) * PointLightPower * brithnessFactor, 1.0f);
    }

    if (SpotLightOn1) {
        vec3 L = SpotLightPos1 - Position;
        float distance = length(L);
        L = normalize(L);

        float brithnessFactor = 1.0f;//dot(L, SpotLight1Dir), CosConeAngle1);
        float dLD = dot(-L, SpotLightDir1);

        if (1.0f == CosConeAngle1) {
            brithnessFactor = 1.0f;
        }
        else if (dLD < CosConeAngle1) {
            brithnessFactor = 0.0f;
        }
        else if ((1.0f - dLD) > concentration1 * (1.0f - CosConeAngle1)) {
            brithnessFactor = ((1.0f - CosConeAngle1) - (1.0f - dLD)) / ((1.0f - concentration1) * (1.0f - CosConeAngle1));
        }
        
        float distanceFactor = max(SpotLightRadius1 - distance, 0.0f) / SpotLightRadius1;
        brithnessFactor *= distanceFactor * distanceFactor;

        float lambertian = countLambertianPart(L, N);
        float specular = 0.0;

        if (lambertian > 0.0) {
            specular = countBlinnPhongPart(L, E, N);
        }
        
        fLightColor += vec4((lambertian + specular) * vec3(SpotLightColor1) * SpotLightPower1 * brithnessFactor, 1.0f);
    }

    if (SpotLightOn2) {
        vec3 L = SpotLightPos2 - Position;
        float distance = length(L);
        L = normalize(L);

        float brithnessFactor = 1.0f;//dot(L, SpotLight1Dir), CosConeAngle1);
        float dLD = dot(-L, SpotLightDir2);

        if (1.0f == CosConeAngle2) {
            brithnessFactor = 1.0f;
        }
        else if (dLD < CosConeAngle2) {
            brithnessFactor = 0.0f;
        }
        else if ((1.0f - dLD) > concentration2 * (1.0f - CosConeAngle2)) {
            brithnessFactor = ((1.0f - CosConeAngle2) - (1.0f - dLD)) / ((1.0f - concentration2) * (1.0f - CosConeAngle2));
        }
        
        float distanceFactor = max(SpotLightRadius2 - distance, 0.0f) / SpotLightRadius2;
        brithnessFactor *= distanceFactor * distanceFactor;

        float lambertian = countLambertianPart(L, N);
        float specular = 0.0;

        if (lambertian > 0.0) {
            specular = countBlinnPhongPart(L, E, N);
        }
        
        fLightColor += vec4((lambertian + specular) * vec3(SpotLightColor2) * SpotLightPower2 * brithnessFactor, 1.0f);
    }

    if (DirectionalLightOn) {
        vec3 L = -DirectionalLightDir;

        float lambertian = countLambertianPart(L, N);
        float specular = 0.0;

        if (lambertian > 0.0) {
            specular = countBlinnPhongPart(L, E, N);
        }
        
        fLightColor += vec4((lambertian + specular) * vec3(DirectionalLightColor) * DirectionalLightPower, 1.0f);
    }


    //color = AND(texture(texture_diffuse1, TexCoords), fLightColor);//* inColor;
    color = texture(texture_diffuse1, TexCoords) * vec4(vec3(fLightColor), 1.0f);//* inColor;
    //color = vec4(1, 1, 1, 1) * fLightColor;//* inColor;
    //color = texture(texture_diffuse1, TexCoords);//* inColor;
}