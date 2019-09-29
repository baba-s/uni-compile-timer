using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KoganeUnityLib
{
	internal sealed class CompileTimer : EditorWindow
	{
		[SerializeField] private List<string> m_list      = new List<string>();
		[SerializeField] private long         m_startTime = 0;

		private bool    m_isCompiling;
		private Vector2 m_scrollPos;

		[MenuItem( "Window/Compile Timer" )]
		public static void Init()
		{
			GetWindow<CompileTimer>( "Compile Timer" );
		}

		private void OnGUI()
		{
			if ( GUILayout.Button( "Clear" ) )
			{
				m_list.Clear();
			}

			m_scrollPos = EditorGUILayout.BeginScrollView( m_scrollPos );

			for ( var i = m_list.Count - 1; i >= 0; i-- )
			{
				var text = m_list[ i ];
				EditorGUILayout.LabelField( text );
			}

			EditorGUILayout.EndScrollView();
		}

		private void OnEnable()
		{
			EditorApplication.update += OnUpdate;
		}

		private void OnDisable()
		{
			EditorApplication.update -= OnUpdate;
		}

		private void OnUpdate()
		{
			var isCompiling = EditorApplication.isCompiling;
			var isStarted   = !m_isCompiling && isCompiling;
			var isEnded     = m_isCompiling && !isCompiling;
			var millisecond = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

			if ( isStarted )
			{
				m_isCompiling = true;
				m_startTime   = millisecond;
			}
			else if ( isEnded )
			{
				m_isCompiling = false;

				var elapsedTime     = ( millisecond - m_startTime ) / 1000f;
				var nowText         = DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" );
				var elapsedTimeText = elapsedTime.ToString( "0.###" ) + " 秒";

				m_list.Add( nowText + " : " + elapsedTimeText );

				Repaint();
			}
		}
	}
}