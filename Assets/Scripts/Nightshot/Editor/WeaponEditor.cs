using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon))]

public class WeaponEditor : Editor
{
    Weapon weapon;

    void OnEnable()
    {
        weapon = (Weapon)target;
    }

    public override void OnInspectorGUI()
    {
        weapon.weaponName = EditorGUILayout.TextField("Name", weapon.weaponName);

        GUILayout.Label("Weapon type", EditorStyles.boldLabel);
        weapon.type = (Weapon.weaponType)EditorGUILayout.EnumPopup("Item Type", weapon.type);
        EditorGUILayout.Space();

        GUILayout.Label("Weapon data", EditorStyles.boldLabel);
        weapon.numberOfUsageMax = EditorGUILayout.IntField("Number of usage", weapon.numberOfUsageMax);
        weapon.attackDamage = EditorGUILayout.IntField("Damage",weapon.attackDamage);
        weapon.maxAmmo = EditorGUILayout.IntField("Max ammo", weapon.maxAmmo);
        weapon.fireRate = EditorGUILayout.FloatField("Fire rate", weapon.fireRate);
        weapon.burstCount = EditorGUILayout.IntField("Burst count", weapon.burstCount);
        if (weapon.burstCount > 1)
            weapon.burstSpacing = EditorGUILayout.FloatField("Burst spacing", weapon.burstSpacing);
        EditorGUILayout.Space();

        GUILayout.Label("Weapon visual", EditorStyles.boldLabel);
        weapon.weaponMesh = EditorGUILayout.ObjectField("Weapon mesh",weapon.weaponMesh, typeof(Mesh), false);
        weapon.weaponMaterial = EditorGUILayout.ObjectField("Weapon material", weapon.weaponMaterial, typeof(Material), false);
        weapon.muzzleFlash = EditorGUILayout.ObjectField("Muzzle flash", weapon.muzzleFlash, typeof(GameObject), false);
        weapon.impactEffect = EditorGUILayout.ObjectField("Impact effect", weapon.impactEffect, typeof(GameObject), false);
        weapon.firePointOffset = EditorGUILayout.Vector3Field("Fire point position offset", weapon.firePointOffset);
        EditorGUILayout.Space();

        GUILayout.Label("ScreenShake", EditorStyles.boldLabel);
        weapon.screenshakeDuration = EditorGUILayout.FloatField("Screenshake duration", weapon.screenshakeDuration);
        weapon.screenshakeMagnitude = EditorGUILayout.FloatField("Screenshake magnitude", weapon.screenshakeMagnitude);
        EditorGUILayout.Space();


        if (weapon.type == Weapon.weaponType.hitscanRay)
        {
            GUILayout.Label("This weapon uses instant hitscan in a ray");
            GUILayout.Label("Dispertion", EditorStyles.boldLabel);
            weapon.dispertion = EditorGUILayout.FloatField("Dispertion rate", weapon.dispertion);
            weapon.weaponRange = EditorGUILayout.FloatField("Range", weapon.weaponRange);
            weapon.bulletTravelTime = EditorGUILayout.FloatField("Bullet trail travel time", weapon.bulletTravelTime);
            weapon.bulletTrail = EditorGUILayout.ObjectField("Bullet Trail", weapon.bulletTrail, typeof(GameObject), false);
        }

        else if (weapon.type == Weapon.weaponType.projectile)
        {
            GUILayout.Label("This weapon uses delayed projectiles");
            GUILayout.Label("Projectile", EditorStyles.boldLabel);
            weapon.constantSpeed = EditorGUILayout.Toggle("Does the projectile have constant speed", weapon.constantSpeed);
            weapon.projectileSpeed = EditorGUILayout.FloatField("Projectile speed", weapon.projectileSpeed);
            weapon.projectilePrefab = EditorGUILayout.ObjectField("Projectile prefab", weapon.projectilePrefab, typeof(GameObject), false);
        }

        if (weapon.type == Weapon.weaponType.hitscanZone)
        {
            GUILayout.Label("This weapon uses instant hitscan in a cone");
            GUILayout.Label("Angle", EditorStyles.boldLabel);
            weapon.angle = EditorGUILayout.FloatField("Fire angle", weapon.angle);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(weapon);
            AssetDatabase.SaveAssets();
            Debug.Log(weapon.name + " has been saved");
        }
    }
}
